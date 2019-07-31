using System;
using System.Text;

namespace EC_to_VSP_EDI {
    public class SubHeader {
        private const string SegmentIDGS = "GS";
        private const string FunctionalIDCode = "BE";
        private const string SenderID = "DDPPGGGGG";
        private const string ReceiverID = "942411167";
        private string GSDate;
        private string GSTime;
        private uint GroupControlNumber;
        private const char ResponsibleAgencyCode = 'X';
        private const string VersionReleaseCode = "005010X220A1";
        private const string SegmentIDST = "ST";
        private const string TransactionIDCode = "834";
        private string TransactionSetControlNumber;
        private const string ImplementationConventionReference = "005010X220A1";
        private const string SegmentIDBGN = "BGN";
        private string TransactionSetPurpose;//BGN01
        private const string ReferenceNumber = "";//BGN02
        private string BGNDate;//BGN03
        private string BGNTime;//BGN04
        private const string TimeCode_BGN = "PT"; //BGN05
        private string ReferenceIdentification;//BGN006
        private const string NotUsed_BGN07 = "";
        private const char ActionCode = '4';//BGN08
        private const string Empty_BGN09 = "";//BGN09
        private const string SegmentIDRef = "REF";
        private const string RefReferenceNumberQualifier = "38";//REF01
        private const string RefReferenceNumber = "PPGGGGG";//REF02

        //DTP
        private const string SegmentID_DTP = "DTP";
        private const string DateTimeQualifier_DTP01 = "007";
        private const string DateTimePeriodFormatQualifier_DTP02 = "D8";
        private string DateTimePeriod_DTOP03;

        private const string N1SegmentID = "N1";
        private const string EntityIdentCodeSponser = "P5";
        private const string PlanSponser = "TDS Group";
        private const string N1IdentificationCodeQualifier = "FI";
        private const string Name_N1A = "Campbell Union HSD";
        private const string N1BEntityIdentifierCode = "IN";
        private const string N1BName = "Delta Dental of California";
        private const string N1BIdentificationCode = "94-1632821";
        private const string N1CEntityIdentifierCode = "TV";
        private const string N1CName = "TDS Group";
        private const string N1BIdentificationCodeQualifier = "FI";
        private const string N1CIdentificationCode = "30-0369656";
        private const string SegmentTerminator = "~";

        public SubHeader() {
            GSDate = DateTime.Now.ToString("yyyyMMdd");
            GSTime = DateTime.Now.ToString("hhmm");
            GroupControlNumber = InterchangeTracker.GetInterchangeNumber();
            TransactionSetControlNumber = InterchangeTracker.GetInterchangeNumber().ToString().PadLeft(4,'0');
            TransactionSetPurpose = Form1.enrollType;
            BGNDate = GSDate;
            BGNTime = GSTime;
            DateTimePeriod_DTOP03 = GSDate;
        }

        public new string ToString() {
            StringBuilder tempSB = new StringBuilder();
            //GS
            tempSB.AppendLine(SegmentIDGS + '*' + FunctionalIDCode + '*' + SenderID + '*' + ReceiverID + '*' + GSDate + '*' + GSTime + '*' +
                GroupControlNumber + '*' + ResponsibleAgencyCode + '*' + VersionReleaseCode + SegmentTerminator);
            //ST
            tempSB.AppendLine(SegmentIDST + '*' + TransactionIDCode + '*' + TransactionSetControlNumber + '*' +
                ImplementationConventionReference + SegmentTerminator);
            //BGN
            tempSB.AppendLine(SegmentIDBGN + '*' + TransactionSetPurpose + '*' + ReferenceNumber + '*' + BGNDate + '*' + BGNTime + '*' + TimeCode_BGN + '*' +
                ((TransactionSetPurpose != TransactionSetPurposes.Original) ? "****" : ("*" + TransactionSetPurpose + "*")) +
                NotUsed_BGN07 + '*' + ActionCode + '*' + Empty_BGN09 + SegmentTerminator);
            //Ref
            tempSB.AppendLine(SegmentIDRef + '*' + RefReferenceNumberQualifier + '*' + ReferenceNumber + SegmentTerminator);
            //DTP
            tempSB.AppendLine(SegmentID_DTP + '*' + DateTimeQualifier_DTP01 + '*' + DateTimePeriodFormatQualifier_DTP02 + '*' +
                DateTimePeriod_DTOP03 + SegmentTerminator);
            //N1A
            tempSB.AppendLine(N1SegmentID + '*' + EntityIdentCodeSponser + '*' + Name_N1A + '*' + 
                N1IdentificationCodeQualifier + '*' + SenderID + SegmentTerminator);
            //N1B
            tempSB.AppendLine(N1SegmentID + '*' + N1BEntityIdentifierCode + '*' + N1BName + '*' + 
                N1IdentificationCodeQualifier + '*' + ReceiverID + SegmentTerminator);
            //N1C
            tempSB.AppendLine(N1SegmentID + '*' + N1CEntityIdentifierCode + '*' + N1CName + '*' + N1IdentificationCodeQualifier + '*' + 
                SenderID + SegmentTerminator);

            return tempSB.ToString();
        }
    }
}
