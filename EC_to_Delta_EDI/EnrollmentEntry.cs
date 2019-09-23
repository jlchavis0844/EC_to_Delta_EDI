using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace EC_to_VSP_EDI {

    public class EnrollmentEntry {
        public const char SegmentTerminator = '~';

        public const string PLANCODE = "PP";
        public const string GROUPID = "07012";

        //INS
        public const string SegmentID_INS = "INS";
        public       string SubscriberIndicator_INS01;
        public       string       IndividualRelationshipCode_INS02;
        public const string MaintenanceTypeCode_INS03 = "030";
        public const string MaintenanceReasonCode_INS04 = "";
        public       string BenefitStatusCode_INS05;
        public const string INS06 = "";
        public const string INS07 = "";
        public       string EmploymentStatusCode_INS08;
        public       string StudentStatusCode_INS09;
        public       string HandicappedIndicator_INS10;
        public const string INS11 = "";
        public const string INS12 = "";
        public const string INS13 = "";
        public const string INS14 = "";
        public const string INS15 = "";
        public const string INS16 = "";
        public const string BirthSequence_INS17 = "";


        //REF
        public const string SegmentID_REF = "REF";
        public const string ReferenceNumberQualifier_REF01 = "0F"; //SSN
        public string       ReferenceNumber_REF02;
        public const string ReferenceNumberQualifier2_REF01 = "1L";
        public       string ReferenceNumber2_REF02 = "";
        public string       ReferenceNumberQualifier3_REF01;
        public string       ReferenceNumber3_REF02;
        public const string ReferenceNumberQualifierVSP_REF01 = "DX"; //VSP division
        public string       ReferenceNumberVSP_REF02;

        //NM1
        public const string SegmentID_NM1 = "NM1";
        public const string EntityIdentifierCode_NM101 = "IL";
        public const char EntityTypeQualifier_NM102 = '1';
        public string NameLast_NM103;
        public string NameFirst_NM104;
        public char NameInitial_NM105;
        public const string NM106 = "";
        public const string NM107 = "";
        public const string IdentificationCodeQualifier_NM108 = "34";
        public string IdentificationCode_NM109;//Member SSN

        //PER
        public const string SegmentID_PER = "PER";
        public const string ContactFunctionCode_PER01 = "IP";
        public string ContactName_PER02;
        public string CommunicationNumberQualifier_PER03;
        public string CommunicationNumber_PER04;
        public const string CommunicationNumberQualifier_PER05 = "EM";
        public string CommunicationNumber_PER06;
        //public const string CommunicationNumberQualifier_PER06 = "EM";
        //public string CommunicationNumber_PER08;

        //N3
        public const string SegmentID_N3 = "N3";
        public string ResidenceAddressLine1_N301;
        public string ResidenceAddressLine2_N302;

        //N4
        public const string SegmentID_N4 = "N4";
        public string ResidenceCity_N401;
        public string ResidenceState_N402;
        public string ResidenceZip_N403;
        public string CountryCode_N404 = "";

        //DMG
        public const string SegmentID_DMG = "DMG";
        public const string DateTimeFormatQualifier_DMG01 = "D8";
        public string DatetimePeriod_DMG02;
        public char GenderCode_DMG03;
        public char MaritalStatusCode_DMG04;
        public char RaceCode_DMG05;
        //public char CitizenshipStatusCode_DMG06;

        //LUI
        public const string SegmentID_LIU = "LUI";
        public const string IdentificationCodeQualifier_LUI01 = "LD";
        public const string IdentificationCode_LUI02 = "ENG";
        public const string LanguageDescription_LUI03 = "English";

        //HD
        public const string SegmentID_HD = "HD";
        public const string MaintenanceTypeCode_HD01 = "021";
        public const string Blank_HD02 = "";
        public const string InsuranceLineCode_HD03 = "DEN";
        public       string PlanCoverageDescription_HD04;
        public       string CoverageLevelCode_HD05;

        //DTP
        public const string SegmentID_DTP = "DTP";
        public const string BenefitStartDate_DTP01 = "348";
        public const string BenefitEndDate_DTP01 = "349";
        //public const string CoverageLevelChange_DTP01 = "303";
        public const string DateTimeFormat_DTP02 = "D8";
        public string DateTimePeriod_Start_DTP03;
        public string DateTimePeriod_End_DTP03;


        //Constructor
        public EnrollmentEntry(CensusRow row) {
            if (row.RelationshipCode == "0") {
                SubscriberIndicator_INS01 = "Y";
                StudentStatusCode_INS09 = "";
            } else {
                SubscriberIndicator_INS01 = "N";
                if(row.StudentStatus== "Full-Time") {
                    StudentStatusCode_INS09 = "F";
                } else if(row.StudentStatus == "Part-Time") {
                    StudentStatusCode_INS09 = "P";
                } else {
                    StudentStatusCode_INS09 = "";
                }
            }

            if(row.JobClass.Contains("CHSTA") || row.JobClass.Contains("CACE")) {
                ReferenceNumber2_REF02 = PLANCODE + GROUPID + "00510";
            } else if(row.JobClass.Contains("CSEA")) {
                ReferenceNumber2_REF02 = PLANCODE + GROUPID + "00508";
            } else if(row.JobClass.Contains("SEIU")) {
                ReferenceNumber2_REF02 = PLANCODE + GROUPID + "00511";
            } else if(row.JobClass.Contains("CLASS-MGMT")) {
                ReferenceNumber2_REF02 = PLANCODE + GROUPID + "00512";
            } else if(row.JobClass.Contains("CERT-MGMT")) {
                ReferenceNumber2_REF02 = PLANCODE + GROUPID + "00513";
            } else {
                ReferenceNumber2_REF02 = PLANCODE + GROUPID + "00000";
            }

            HandicappedIndicator_INS10 = row.Disabled == "Yes" ? "Y" : "N";
            IndividualRelationshipCode_INS02 = RelationshipTranslation(row.RelationshipCode);
            BenefitStatusCode_INS05 = "A";
            EmploymentStatusCode_INS08 = EmploymentStatusCodeTranslation(row.EmployeeStatus, row.JobClass);

            var memberSSN = (from record in Form1.records
                             where record.EID == row.EID && record.RelationshipCode == "0"
                             select record.SSN).First().ToString().Replace("-", "");

            if (memberSSN != null && memberSSN != "") {
                //ReferenceNumber_REF02 = row.SSN.Replace("-", "");
                ReferenceNumber_REF02 = memberSSN;
            } else {
                //ReferenceNumber_REF02 = "         ";
                Form1.log.Error("ERR: " + (++Form1.errorCounter) + "\tMissing SSN for the following:\n" + row.ToString());
            }

            ReferenceNumberVSP_REF02 = row.Division;
            NameLast_NM103 = row.LastName;
            NameFirst_NM104 = row.FirstName;

            if(row.MiddleName.Length > 0)
                NameInitial_NM105 = row.MiddleName[0];

            if(row.SSN != null && row.SSN != "")
                IdentificationCode_NM109 = row.SSN.Replace("-","");


            ContactName_PER02 = (row.FirstName + " " + row.LastName).Trim();
            CommunicationNumberQualifier_PER03 = "HP";

            if(row.PersonalPhone != null && row.PersonalPhone != "")
                CommunicationNumber_PER04 = Regex.Replace(row.PersonalPhone, "[^0-9]", "");

            CommunicationNumber_PER06 = row.Email;
            ResidenceAddressLine1_N301 = row.Address1;
            ResidenceAddressLine2_N302 = row.Address2;
            ResidenceCity_N401 = row.City;
            ResidenceState_N402 = "CA";
            ResidenceZip_N403 = row.Zip;

            if (row.BirthDate != null && row.BirthDate != "") {
                DatetimePeriod_DMG02 = DateTime.Parse(row.BirthDate).ToString("yyyyMMdd");
            }

            if (row.Gender == "Male") {
                GenderCode_DMG03 = GenderCodes.Male;
            } else if (row.Gender == "Female") {
                GenderCode_DMG03 = GenderCodes.Female;
            } else {
                GenderCode_DMG03 = GenderCodes.Unknown;
            }

            MaritalStatusCode_DMG04 = MaritalTranslation(row.MaritalStatus);
            CoverageLevelCode_HD05 = CoverageTranslation(row.CoverageDetails);
            if(row.PlanEffectiveStartDate != null && row.PlanEffectiveStartDate != "") {
                DateTimePeriod_Start_DTP03 = DateTime.Parse(row.PlanEffectiveStartDate).ToString("yyyyMMdd");
            }

            PlanCoverageDescription_HD04 = Form1.dentalPlans[row.PlanImportID];
            if(row.CoverageDetails == "Terminated") {
                DateTimePeriod_End_DTP03 = DateTime.Parse(row.PlanEffectiveEndDate).ToString("yyyyMMdd");
            }
        }

        public new string ToString() {
            StringBuilder sb = new StringBuilder();
            
            //INS
            sb.AppendLine(SegmentID_INS + '*' + SubscriberIndicator_INS01 + '*' + IndividualRelationshipCode_INS02 + '*' + MaintenanceTypeCode_INS03 + '*' +
                MaintenanceReasonCode_INS04 + '*' + BenefitStatusCode_INS05 +'*' + INS06 + '*' + INS07 + '*' + EmploymentStatusCode_INS08 + '*' +
                StudentStatusCode_INS09 + '*' + HandicappedIndicator_INS10 + '*' + INS11 + '*' + INS12 + '*' + INS13 + '*' + INS14 + '*' +
                INS15 + '*' + INS16 + '*' + BirthSequence_INS17 + SegmentTerminator);
            
            //REFA
            sb.AppendLine(SegmentID_REF + '*' + ReferenceNumberQualifier_REF01 + '*' + ReferenceNumber_REF02 + SegmentTerminator);

            //REFB
            sb.AppendLine(SegmentID_REF + '*' + ReferenceNumberQualifier2_REF01 + '*' + ReferenceNumber2_REF02 + SegmentTerminator);

            //REFC
            //sb.AppendLine(SegmentID_REF + '*' + ReferenceNumberQualifierVSP_REF01 + '*' +  ReferenceNumberVSP_REF02 + SegmentTerminator);
            
            //NM1
            sb.AppendLine(SegmentID_NM1 + '*' +EntityIdentifierCode_NM101 + '*' + EntityTypeQualifier_NM102 + '*' + NameLast_NM103 + '*' 
                + NameFirst_NM104 + '*' + NameInitial_NM105 + '*' + NM106 + '*' + NM107 + '*' + 
                //Do not send NM108 and NM109 if MEMBER ssn is not given
                (String.IsNullOrEmpty(IdentificationCode_NM109) ? "" :(IdentificationCodeQualifier_NM108 + '*' + IdentificationCode_NM109)) + SegmentTerminator);
            
            //PER
            sb.AppendLine(SegmentID_PER + '*' + ContactFunctionCode_PER01 + '*' + ContactName_PER02 + '*' + CommunicationNumberQualifier_PER03 + '*' + 
                CommunicationNumber_PER04 + SegmentTerminator);
            //N3
            sb.AppendLine(SegmentID_N3 + '*' + ResidenceAddressLine1_N301 + '*' + ResidenceAddressLine2_N302 + SegmentTerminator);
            
            //N4
            sb.AppendLine(SegmentID_N4 + '*' + ResidenceCity_N401 + '*' + ResidenceState_N402 + '*' + ResidenceZip_N403 + "*" + 
                CountryCode_N404 +SegmentTerminator);
            
            //DMG
            sb.AppendLine(SegmentID_DMG + '*' + DateTimeFormatQualifier_DMG01 + '*' + DatetimePeriod_DMG02 + '*' + 
                GenderCode_DMG03 +'*' + MaritalStatusCode_DMG04 + SegmentTerminator);
            
            //LIU
            //We good

            //2300 
            //HD
            sb.AppendLine(SegmentID_HD + '*' + MaintenanceTypeCode_HD01 + '*' + Blank_HD02 + '*' + InsuranceLineCode_HD03 + '*' + 
                PlanCoverageDescription_HD04 + '*' + CoverageLevelCode_HD05 + SegmentTerminator);
            
            //DTP start
            sb.AppendLine(SegmentID_DTP + '*' + BenefitStartDate_DTP01 + '*' + DateTimeFormat_DTP02 + '*' + DateTimePeriod_Start_DTP03 + SegmentTerminator);
            
            //DTP end
            if(!String.IsNullOrEmpty(DateTimePeriod_End_DTP03)) {
                sb.AppendLine(SegmentID_DTP + '*' + BenefitEndDate_DTP01 + '*' + DateTimeFormat_DTP02 + '*' + DateTimePeriod_End_DTP03 + SegmentTerminator);
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
            } else return null;
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
            } else if(status == "Terminated") {
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
                    return "";
            }
        }
    }
}
