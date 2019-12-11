// <copyright file="EnrollmentEntry.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EC_to_VSP_EDI {
    using System;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public class EnrollmentEntry {
        private const char SegmentTerminator = '~';

        private const string PLANCODE = "CA";
        private const string GROUPID = "07012";

        // INS
        private const string SegmentIDINS = "INS";
        private readonly string subscriberIndicatorINS01;
        private readonly string individualRelationshipCodeINS02;
        private readonly string MaintenanceTypeCodeINS03;
        private readonly string MaintenanceReasonCodeINS04 = string.Empty;
        private readonly string benefitStatusCodeINS05;
        private readonly string INS06 = string.Empty;
        private readonly string INS07 = string.Empty;
        private readonly string employmentStatusCodeINS08;
        private readonly string studentStatusCodeINS09;
        private readonly string handicappedIndicatorINS10;
        private readonly string INS11 = string.Empty;
        private readonly string INS12 = string.Empty;
        private readonly string INS13 = string.Empty;
        private readonly string INS14 = string.Empty;
        private readonly string INS15 = string.Empty;
        private readonly string INS16 = string.Empty;
        private readonly string BirthSequenceINS17 = string.Empty;

        // REF
        private const string SegmentIDREF = "REF";
        private const string ReferenceNumberQualifierREF01 = "0F"; // SSN
        private readonly string referenceNumberREF02;
        private const string ReferenceNumberQualifier2REF01 = "1L";
        private readonly string referenceNumber2REF02 = string.Empty;
        // private readonly string referenceNumberQualifier3REF01;
        // private readonly string referenceNumber3REF02;
        // private const string ReferenceNumberQualifierVSPREF01 = "DX"; // VSP division
        private readonly string referenceNumberVSPREF02;

        // NM1
        private const string SegmentIDNM1 = "NM1";
        private const string EntityIdentifierCodeNM101 = "IL";
        private const char EntityTypeQualifierNM102 = '1';
        private readonly string nameLastNM103;
        private readonly string nameFirstNM104;
        private readonly string nameInitialNM105;
        private readonly string NM106 = string.Empty;
        private readonly string NM107 = string.Empty;
        private const string IdentificationCodeQualifierNM108 = "34";
        private readonly string identificationCodeNM109; // Member SSN

        // PER
        private const string SegmentIDPER = "PER";
        private const string ContactFunctionCodePER01 = "IP";
        private readonly string contactNamePER02;
        private readonly string communicationNumberQualifierPER03;
        private readonly string communicationNumberPER04;
        private const string CommunicationNumberQualifierPER05 = "EM";
        private readonly string communicationNumberPER06;

        // public const string CommunicationNumberQualifier_PER06 = "EM";
        // public string CommunicationNumber_PER08;

        // N3
        private const string SegmentIDN3 = "N3";
        private readonly string residenceAddressLine1N301;
        private readonly string residenceAddressLine2N302;

        // N4
        private const string SegmentIDN4 = "N4";
        private readonly string residenceCityN401;
        private readonly string residenceStateN402;
        private readonly string residenceZipN403;
        private readonly string countryCodeN404 = string.Empty;

        // DMG
        private const string SegmentIDDMG = "DMG";
        private const string DateTimeFormatQualifierDMG01 = "D8";
        private readonly string datetimePeriodDMG02;
        private readonly char genderCodeDMG03;
        private readonly char maritalStatusCodeDMG04;
        // private readonly char raceCodeDMG05;

        // public char CitizenshipStatusCode_DMG06;

        // LUI
        private const string SegmentIDLIU = "LUI";
        private const string IdentificationCodeQualifierLUI01 = "LD";
        private const string IdentificationCodeLUI02 = "ENG";
        private const string LanguageDescriptionLUI03 = "English";

        // HD
        private const string SegmentIDHD = "HD";
        private readonly string MaintenanceTypeCodeHD01;
        private readonly string BlankHD02 = string.Empty;
        private const string InsuranceLineCodeHD03 = "DEN";
        private readonly string planCoverageDescriptionHD04;
        private readonly string coverageLevelCodeHD05;

        // DTP
        private const string SegmentIDDTP = "DTP";
        private const string BenefitStartDateDTP01 = "348";
        private const string BenefitEndDateDTP01 = "349";

        // public const string CoverageLevelChange_DTP01 = "303";
        private const string DateTimeFormatDTP02 = "D8";
        private readonly string dateTimePeriodStartDTP03;
        private readonly string dateTimePeriodEndDTP03;

        // Constructor
        public EnrollmentEntry(CensusRow row) {
            if (row.LastName == "Baeza")
                Console.WriteLine("Found it");

            if (row.RelationshipCode == "0") {
                this.subscriberIndicatorINS01 = "Y";
                this.studentStatusCodeINS09 = string.Empty;
            } else {
                this.subscriberIndicatorINS01 = "N";
                if (row.StudentStatus == "Full-Time") {
                    this.studentStatusCodeINS09 = "F";
                } else if (row.StudentStatus == "Part-Time") {
                    this.studentStatusCodeINS09 = "P";
                } else {
                    this.studentStatusCodeINS09 = string.Empty;
                }
            }

            //Workaround to give emp Job Class to Depend
            var jClass = Form1.Records.Where(x => x.EID == row.EID
                && x.RelationshipCode == "0").Select(y => y.JobClass).First().ToString();
            row.JobClass = jClass;

            if (row.JobClass.Contains("CHSTA") || row.JobClass.Contains("CACE")) {
                this.referenceNumber2REF02 = PLANCODE + GROUPID + "00510";
            } else if (row.JobClass.Contains("CSEA")) {
                this.referenceNumber2REF02 = PLANCODE + GROUPID + "00508";
            } else if (row.JobClass.Contains("SEIU")) {
                this.referenceNumber2REF02 = PLANCODE + GROUPID + "00511";
            } else if (row.JobClass.Contains("CLASS-MGMT")) {
                this.referenceNumber2REF02 = PLANCODE + GROUPID + "00512";
            } else if (row.JobClass.Contains("CERT-MGMT")) {
                this.referenceNumber2REF02 = PLANCODE + GROUPID + "00513";
            } else {
                this.referenceNumber2REF02 = PLANCODE + GROUPID + "00000";
            }

            this.handicappedIndicatorINS10 = row.Disabled == "Yes" ? "Y" : "N";
            this.individualRelationshipCodeINS02 = this.RelationshipTranslation(row.RelationshipCode);
            this.benefitStatusCodeINS05 = "A";
            this.employmentStatusCodeINS08 = this.EmploymentStatusCodeTranslation(row.EmployeeStatus, row.JobClass);

            var memberSSN = (from record in Form1.Records
                             where record.EID == row.EID && record.RelationshipCode == "0"
                             select record.SSN).First().ToString().Replace("-", string.Empty);

            if (memberSSN != null && memberSSN != string.Empty) {
                // ReferenceNumber_REF02 = row.SSN.Replace("-", "");
                this.referenceNumberREF02 = memberSSN;
            } else {
                // ReferenceNumber_REF02 = "         ";
                Form1.Log.Error("ERR: " + (++Form1.ErrorCounter) + "\tMissing SSN for the following:\n" + row.ToString());
            }

            this.referenceNumberVSPREF02 = row.Division;
            this.nameLastNM103 = row.LastName;
            this.nameFirstNM104 = row.FirstName;

            if (row.MiddleName.Length > 0) {
                this.nameInitialNM105 = row.MiddleName.Substring(0, 1);
            } else this.nameInitialNM105 = string.Empty;

            if (row.SSN != null && row.SSN != string.Empty) {
                this.identificationCodeNM109 = row.SSN.Replace("-", string.Empty);
            }

            this.contactNamePER02 = (row.FirstName + " " + row.LastName).Trim();
            this.communicationNumberQualifierPER03 = "HP";

            if (row.PersonalPhone != null && row.PersonalPhone != string.Empty) {
                this.communicationNumberPER04 = Regex.Replace(row.PersonalPhone, "[^0-9]", string.Empty);
            }

            this.communicationNumberPER06 = row.Email;
            this.residenceAddressLine1N301 = row.Address1;
            this.residenceAddressLine2N302 = row.Address2;
            this.residenceCityN401 = row.City;
            this.residenceStateN402 = "CA";
            this.residenceZipN403 = row.Zip;

            if (row.BirthDate != null && row.BirthDate != string.Empty) {
                this.datetimePeriodDMG02 = DateTime.Parse(row.BirthDate).ToString("yyyyMMdd");
            }

            if (row.Gender == "Male") {
                this.genderCodeDMG03 = GenderCodes.Male;
            } else if (row.Gender == "Female") {
                this.genderCodeDMG03 = GenderCodes.Female;
            } else {
                this.genderCodeDMG03 = GenderCodes.Unknown;
            }

            this.maritalStatusCodeDMG04 = this.MaritalTranslation(row.MaritalStatus);
            this.coverageLevelCodeHD05 = this.CoverageTranslation(row.CoverageDetails);
            //if (row.PlanEffectiveStartDate != null && row.PlanEffectiveStartDate != string.Empty) {
            //    this.dateTimePeriodStartDTP03 = DateTime.Parse(row.PlanEffectiveStartDate).ToString("yyyyMMdd");
            //}

            this.planCoverageDescriptionHD04 = row.PlanAdminName;
            if (row.CoverageDetails == "Terminated") {
                this.dateTimePeriodEndDTP03 = DateTime.Parse(row.PlanEffectiveEndDate).ToString("yyyyMMdd");
            }

            //if (row.CoverageDetails == "Terminated") {
            //    this.MaintenanceReasonCodeINS04 = "024";
            //} else if (row.NewBusiness == "Yes") {
            //    this.MaintenanceReasonCodeINS04 = "021";
            //} else {
            //    this.MaintenanceReasonCodeINS04 = "001";
            //}
            MaintenanceReasonCodeINS04 = string.Empty;

            if (row.Drop == "TRUE") {
                MaintenanceTypeCodeINS03 = "024"; // Terminate
                this.dateTimePeriodEndDTP03 = "20191231";
                this.dateTimePeriodStartDTP03 = "20190101";
            } else if (row.Add == "TRUE") {
                MaintenanceTypeCodeINS03 = "021"; //Add
                this.dateTimePeriodStartDTP03 = "20200101";
            } else { 
                MaintenanceTypeCodeINS03 = "001"; // Maints
                this.dateTimePeriodStartDTP03 = "20200101";
            } // Update

            MaintenanceTypeCodeHD01 = MaintenanceTypeCodeINS03;
        }

        public new string ToString() {
            StringBuilder sb = new StringBuilder();

            // INS
            sb.AppendLine(SegmentIDINS + '*' + this.subscriberIndicatorINS01 + '*' + this.individualRelationshipCodeINS02 + '*' + MaintenanceTypeCodeINS03 + '*' +
                MaintenanceReasonCodeINS04 + '*' + this.benefitStatusCodeINS05 + '*' + INS06 + '*' + INS07 + '*' + this.employmentStatusCodeINS08 + '*' +
                this.studentStatusCodeINS09 + '*' + this.handicappedIndicatorINS10 + '*' + INS11 + '*' + INS12 + '*' + INS13 + '*' + INS14 + '*' +
                INS15 + '*' + INS16 + '*' + BirthSequenceINS17 + SegmentTerminator);

            // REFA
            sb.AppendLine(SegmentIDREF + '*' + ReferenceNumberQualifierREF01 + '*' + this.referenceNumberREF02 + SegmentTerminator);

            // REFB
            sb.AppendLine(SegmentIDREF + '*' + ReferenceNumberQualifier2REF01 + '*' + this.referenceNumber2REF02 + SegmentTerminator);

            // REFC
            // sb.AppendLine(SegmentID_REF + '*' + ReferenceNumberQualifierVSP_REF01 + '*' +  ReferenceNumberVSP_REF02 + SegmentTerminator);

            // NM1
            sb.AppendLine(SegmentIDNM1 + '*' + EntityIdentifierCodeNM101 + '*' + EntityTypeQualifierNM102 + '*' + this.nameLastNM103 + '*'
                + this.nameFirstNM104 + '*' + this.nameInitialNM105 + '*' + NM106 + '*' + NM107 + '*' +

                // Do not send NM108 and NM109 if MEMBER ssn is not given
                (string.IsNullOrEmpty(this.identificationCodeNM109) ? string.Empty : (IdentificationCodeQualifierNM108 + '*' + this.identificationCodeNM109)) + SegmentTerminator);

            // PER
            sb.AppendLine(SegmentIDPER + '*' + ContactFunctionCodePER01 + '*' + this.contactNamePER02 + '*' + this.communicationNumberQualifierPER03 + '*' +
                this.communicationNumberPER04 + SegmentTerminator);

            // N3
            sb.AppendLine(SegmentIDN3 + '*' + this.residenceAddressLine1N301 + '*' + this.residenceAddressLine2N302 + SegmentTerminator);

            // N4
            sb.AppendLine(SegmentIDN4 + '*' + this.residenceCityN401 + '*' + this.residenceStateN402 + '*' + this.residenceZipN403 + "*" +
                this.countryCodeN404 + SegmentTerminator);

            // DMG
            sb.AppendLine(SegmentIDDMG + '*' + DateTimeFormatQualifierDMG01 + '*' + this.datetimePeriodDMG02 + '*' +
                this.genderCodeDMG03 + '*' + this.maritalStatusCodeDMG04 + SegmentTerminator);

            // LIU
            // We good

            // 2300
            // HD
            sb.AppendLine(SegmentIDHD + '*' + MaintenanceTypeCodeHD01 + '*' + BlankHD02 + '*' + InsuranceLineCodeHD03 + '*' +
                this.planCoverageDescriptionHD04 + '*' + this.coverageLevelCodeHD05 + SegmentTerminator);


            //// DTP start
            sb.AppendLine(SegmentIDDTP + '*' + BenefitStartDateDTP01 + '*' + DateTimeFormatDTP02 + '*' + this.dateTimePeriodStartDTP03 + SegmentTerminator);

            //// DTP end
            //if () {
            //    sb.AppendLine(SegmentIDDTP + '*' + BenefitEndDateDTP01 + '*' + DateTimeFormatDTP02 + '*' + this.dateTimePeriodEndDTP03 + SegmentTerminator);
            //}

            //if (MaintenanceTypeCodeINS03 == "001") {//Maints
            //    sb.AppendLine(SegmentIDDTP + '*' + "303" + '*' + DateTimeFormatDTP02 + '*' + this.dateTimePeriodStartDTP03 + SegmentTerminator);
            //} else if (this.MaintenanceTypeCodeINS03 == "024") {//Term
            //    sb.AppendLine(SegmentIDDTP + '*' + "349" + '*' + DateTimeFormatDTP02 + '*' + this.dateTimePeriodStartDTP03 + SegmentTerminator);
            //} else {//Add
            //    sb.AppendLine(SegmentIDDTP + '*' + "348" + '*' + DateTimeFormatDTP02 + '*' + this.dateTimePeriodStartDTP03 + SegmentTerminator);
            //}

            if (this.MaintenanceTypeCodeINS03 == "024") {//Term
                sb.AppendLine(SegmentIDDTP + '*' + "349" + '*' + DateTimeFormatDTP02 + '*' + this.dateTimePeriodEndDTP03 + SegmentTerminator);
            }
                return sb.ToString();
        }

        private string CoverageTranslation(string coverageIn) {
            if (coverageIn.Contains("Employee") && (coverageIn.Contains("Spouse") || coverageIn.Contains("partner")) && coverageIn.Contains("Child")) {
                return CoverageLevels.Family;
            } else if (coverageIn.Contains("Employee") && (coverageIn.Contains("Spouse") || coverageIn.Contains("partner"))) {
                return CoverageLevels.EmployeeSpouse;
            } else if (coverageIn.Contains("Employee") && coverageIn.Contains("Child")) {
                return CoverageLevels.EmployeeCHD;
            } else if (coverageIn.Contains("Employee")) {
                return CoverageLevels.Individual;
            } else {
                return null;
            }
        }

        private string RelationshipTranslation(string relIn) {
            switch (relIn) {
                case "0":
                    return "18";

                case "1":
                    if (relIn.Contains("Part")) {
                        return "53";
                    } else {
                        return "01";
                    }

                default:
                    return "19";
            }
        }

        private char MaritalTranslation(string statusIn) {
            switch (statusIn) {
                case "Married":
                    return MaritalStatusCodes.Married;

                case "Single":
                    return MaritalStatusCodes.Single;

                case "Divorced":
                    return MaritalStatusCodes.Divorced;

                case "Widowed":
                    return MaritalStatusCodes.Widowed;

                case "Domestic Partner":
                    return MaritalStatusCodes.RegisteredDomesticPartner;

                case "Legally Separated":
                    return MaritalStatusCodes.LegallySeparated;

                default:
                    return MaritalStatusCodes.Unreported;
            }
        }

        private string EmploymentStatusCodeTranslation(string status, string empClass) {
            if (status == "Retired") {
                return "RT";
            } else if (status == "Terminated") {
                return "TE";
            }

            switch (empClass) {
                case "CHSTA":
                    return "FT";

                case "CERT-MGMT":
                    return "FT";

                case "CHSTA-80FTE":
                    return "PT";

                case "CSEA":
                    return "FT";

                case "CLASS-MGMT":
                    return "FT";

                case "CSEA-6HR":
                    return "PT";

                case "SEIU":
                    return "FT";

                case "CHSTA-60FTE":
                    return "PT";

                case "SEIU-4HR":
                    return "PT";

                case "CHSTA-85FTE":
                    return "PT";

                case "CACE-30HR+":
                    return "FT";

                default:
                    return string.Empty;
            }
        }
    }
}
