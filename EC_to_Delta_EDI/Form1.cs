// <copyright file="Form1.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EC_to_VSP_EDI {
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Windows.Forms;
    using CsvHelper;
    using log4net;
    using Syroot.Windows.IO;

    public partial class Form1 : Form {
        public static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static string EnrollType;
        public static string InputFile;
        public static Header Header;
        public static SubHeader SubHeader;
        public static Trailer Trailer;
        public static List<EnrollmentEntry> Enrollments = new List<EnrollmentEntry>();
        public static List<CensusRow> Records = new List<CensusRow>();
        public static string OutputFolder;
        public static StringBuilder TextOut;
        public static int ErrorCounter = 0;
        public static Dictionary<string, string> DentalPlans = new Dictionary<string, string>();

        public Form1() {
            this.InitializeComponent();
            this.cbType.SelectedIndex = 0;

            // log4net.Config.XmlConfigurator.Configure();
            string interchangeNumber = InterchangeTracker.GetInterchangeNumber().ToString();
            string dateStr = InterchangeTracker.GetInterchangeDate();
            int year = Convert.ToInt32("20" + dateStr.Substring(0, 2));
            int mon = Convert.ToInt32(dateStr.Substring(2, 2));
            int day = Convert.ToInt32(dateStr.Substring(4, 2));
            string time = InterchangeTracker.GetInterchangeTime();
            int hour = Convert.ToInt32(time.Substring(0, 2));
            int min = Convert.ToInt32(time.Substring(2, 2));

            DateTime dt = new DateTime(year, mon, day, hour, min, 0);

            this.lblInterchangeNumber.Text = "Interchange Number: " + interchangeNumber;
            this.dtPicker.Value = dt;

            DentalPlans["CC63XA"] = "2020 Dental Plan";
            DentalPlans["ERA6R4"] = "2020 Dental Plan";

            Log.Info("Starting form loading at " + DateTime.Now);
        }

        public void BtnLoadFile_Click(object sender, EventArgs e) {
            Log.Info("Load button clicked");
            string type = "csv";

            using (OpenFileDialog ofd = new OpenFileDialog()) {
                //ofd.InitialDirectory = KnownFolders.Downloads.Path;
                ofd.InitialDirectory = @"\\nas3\Shared\RALIM\TDSGroup-Kronos\Campbell\2019 OE\Delta";
                ofd.Filter = type + " files (*." + type + ")| *." + type;
                ofd.FilterIndex = 1;

                if (ofd.ShowDialog() == DialogResult.OK) {
                    InputFile = ofd.FileName;
                    Log.Info(InputFile + " loaded");
                    this.lblFileLocation.Text = InputFile;
                    this.btnProcessEDI.Enabled = true;

                    var tempFile = File.Open(InputFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    using (var reader = new StreamReader(tempFile)) {
                        var csv = new CsvReader(reader);
                        csv.Configuration.HeaderValidated = null;
                        csv.Configuration.HasHeaderRecord = true;
                        csv.Configuration.RegisterClassMap<CensusRowClassMap>();

                        Records = csv.GetRecords<CensusRow>().ToList();
                        //Records = csv.GetRecords<CensusRow>().Where(rec => rec.CoverageDetails != "Waived"
                        //&& DateTime.Parse(rec.PlanEffectiveEndDate) >= DateTime.Now).ToList()
                        //.Where(rec =>
                        //    rec.CoverageDetails != "Waived" &&
                        //    DateTime.Parse(rec.PlanEffectiveEndDate) >= DateTime.Now &&
                        //    rec.PlanType == "Dental").ToList();

                        Log.Info(Records.Count() + " records loaded from Census file.");
                    }
                } else {
                    MessageBox.Show("ERROR LOADING INPUT FILE", "ERROR LOADING INPUT FILE", MessageBoxButtons.OK);
                    Log.Info("No file chosen");
                    Application.Exit();
                }
            }
        }

        public void Button1_Click(object sender, EventArgs e) {
            Enrollments.Clear();
            if (!File.Exists(this.lblFileLocation.Text)) {
                Log.Info("no file found loaded\n" + this.lblFileLocation);
                this.btnProcessEDI.Enabled = false;
                return;
            }

            switch (this.cbType.SelectedIndex) {
                case 1:
                    EnrollType = TransactionSetPurposes.Original;
                    break;

                case 2:
                    EnrollType = TransactionSetPurposes.ReSubmission;
                    break;

                case 3:
                    EnrollType = TransactionSetPurposes.InformationCopy;
                    break;

                default:
                    EnrollType = TransactionSetPurposes.Test;
                    break;
            }

            SubHeader = new SubHeader();
            Header = new Header();

            foreach (var row in Records) {
                Enrollments.Add(new EnrollmentEntry(row));
            }

            Trailer = new Trailer();

            TextOut = new StringBuilder();
            TextOut.AppendLine(Header.ToString());
            TextOut.AppendLine(SubHeader.ToString());

            // Console.WriteLine(header.ToString());
            // Console.WriteLine(subHeader.ToString());
            foreach (var line in Enrollments) {
                TextOut.AppendLine(line.ToString());

                // Console.WriteLine(line.ToString());
            }

            TextOut.AppendLine(Trailer.ToString());
            TextOut = new StringBuilder(TextOut.ToString().Replace("\r\n\r\n", "\r\n"));
            string[] segments = TextOut.ToString().Split('\n');

            string[] SE_Segs = segments[segments.Length - 4].Split('*');
            Console.WriteLine(segments[segments.Length - 4]);

            SE_Segs[1] = (segments.ToList().Count(ew => ew.Length > 3) - 4).ToString();
            string NewSE = SE_Segs[0] + '*' + SE_Segs[1] + '*' + SE_Segs[2];
            segments[segments.Length - 4] = NewSE;

            TextOut.Clear();

            foreach (string seg in segments) {
                string cleanSeg = seg.Replace("\n", "");
                cleanSeg = cleanSeg.Replace("\r", "");
                TextOut.AppendLine(cleanSeg);
            }

            Console.WriteLine(segments[segments.Length - 4]);


            //this.tbTextOut.MaxLength = int.MaxValue;
            TextOut = new StringBuilder(TextOut.ToString());
            this.tbTextOut.Text = TextOut.ToString();
            this.btnOutput.Enabled = true;
            //lblProcessedCnt.Text = Enrollments.Count().ToString() + " enrollments listed";
            //Enrollments.Count().Where();
        }

        private void BtnSaveFile_Click(object sender, EventArgs e) {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select the directory to output files to";
            fbd.ShowNewFolderButton = true;

            // fbd.RootFolder = Environment.SpecialFolder.MyDocuments;
            DialogResult result = fbd.ShowDialog();
            if (result == DialogResult.OK) {
                OutputFolder = fbd.SelectedPath;
                Log.Info("Output directory set to " + OutputFolder);
                this.lblOutPutFolder.Text = OutputFolder;
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e) {
            InterchangeTracker.UpdateInterchange();
            string interchangeNumber = InterchangeTracker.GetInterchangeNumber().ToString();
            string dateStr = InterchangeTracker.GetInterchangeDate();
            int year = Convert.ToInt32("20" + dateStr.Substring(0, 2));
            int mon = Convert.ToInt32(dateStr.Substring(2, 2));
            int day = Convert.ToInt32(dateStr.Substring(4, 2));
            string time = InterchangeTracker.GetInterchangeTime();
            int hour = Convert.ToInt32(time.Substring(0, 2));
            int min = Convert.ToInt32(time.Substring(2, 2));

            DateTime dt = new DateTime(year, mon, day, hour, min, 0);

            this.lblInterchangeNumber.Text = "Interchange Number: " + interchangeNumber;
            this.dtPicker.Value = dt;
            Log.Info("updated interchange to " + InterchangeTracker.ToString());
        }

        private void BtnOutput_Click(object sender, EventArgs e) {
            string outputFileLocation = OutputFolder + @"\t" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
            Log.Info("attempting to save EDI to " + outputFileLocation);
            char[] temp = new char[] { '\r', '\n' };
            try {
                using (StreamWriter file = new StreamWriter(outputFileLocation, false)) {
                    file.WriteLine(TextOut.ToString().TrimEnd(temp));
                    this.lblOutputSave.Text = "Saved to " + outputFileLocation;
                }

                Log.Info("saved output to " + outputFileLocation);
            } catch (Exception ex) {
                ErrorCounter++;
                Log.Error("ERROR\n" + ex);
            }
        }
    }
}
