// <copyright file="SubHeader.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EC_to_VSP_EDI {
    using System;
    using System.Text;

    public class SubHeader {
        private const string SegmentIDGS = "GS";
        private const string FunctionalIDCode = "BE";
        private const string SenderID = "DDCA07012";
        private const string ReceiverID = "942411167";
        private readonly string gSDate;
        private readonly string gSTime;
        private readonly uint groupControlNumber;
        private const char ResponsibleAgencyCode = 'X';
        private const string VersionReleaseCode = "005010X220A1";
        private const string SegmentIDST = "ST";
        private const string TransactionIDCode = "834";
        private readonly string transactionSetControlNumber;
        private const string ImplementationConventionReference = "005010X220A1";

        // BGN
        private const string SegmentIDBGN = "BGN";
        private readonly string transactionSetPurpose; // BGN01
        private readonly string referenceNumber; // BGN02
        private readonly string bGNDate; // BGN03
        private readonly string bGNTime; // BGN04
        private const string TimeCodeBGN = "PT"; // BGN05
        private const string ReferenceIdentificationBGN06 = ""; // BGN06
        private const string NotUsedBGN07 = "";
        private const char ActionCode = '2'; // BGN08
        private const string EmptyBGN09 = ""; // BGN09

        // REF
        private const string SegmentIDRef = "REF";
        private const string RefReferenceNumberQualifier = "38"; // REF01
        private const string RefReferenceNumber = "CA07012"; // REF02

        // DTP
        private const string SegmentIDDTP = "DTP";
        private const string DateTimeQualifierDTP01 = "007";
        private const string DateTimePeriodFormatQualifierDTP02 = "D8";
        private readonly string dateTimePeriodDTOP03;

        private const string N1SegmentID = "N1";
        private const string EntityIdentCodeSponser = "P5";
        private const string PlanSponser = "TDS Group";
        private const string IdentificationCodeN104 = "94-2239786";
        private const string N1IdentificationCodeQualifier = "FI";
        private const string NameN1A = "Campbell Union HSD";
        private const string N1BEntityIdentifierCode = "IN";
        private const string N1BName = "Delta Dental of California";
        private const string N1BIdentificationCode = "94-1632821";
        private const string N1CEntityIdentifierCode = "TV";
        private const string N1CName = "TDS Group";
        private const string N1BIdentificationCodeQualifier = "FI";
        private const string N1CIdentificationCode = "30-0369656";
        private const string SegmentTerminator = "~";

        public SubHeader() {
            this.gSDate = DateTime.Now.ToString("yyyyMMdd");
            this.gSTime = DateTime.Now.ToString("hhmm");
            this.groupControlNumber = InterchangeTracker.GetInterchangeNumber();
            this.transactionSetControlNumber = InterchangeTracker.GetInterchangeNumber().ToString().PadLeft(4, '0');
            this.referenceNumber = this.transactionSetControlNumber;
            this.transactionSetPurpose = Form1.EnrollType;
            this.bGNDate = this.gSDate;
            this.bGNTime = this.gSTime;
            this.dateTimePeriodDTOP03 = this.gSDate;
        }

        public new string ToString() {
            StringBuilder tempSB = new StringBuilder();

            // GS
            tempSB.AppendLine(SegmentIDGS + '*' + FunctionalIDCode + '*' + SenderID + '*' + ReceiverID + '*' + this.gSDate + '*' + this.gSTime + '*' +
                this.groupControlNumber + '*' + ResponsibleAgencyCode + '*' + VersionReleaseCode + SegmentTerminator);

            // ST
            tempSB.AppendLine(SegmentIDST + '*' + TransactionIDCode + '*' + this.transactionSetControlNumber + '*' +
                ImplementationConventionReference + SegmentTerminator);

            // BGN
            tempSB.AppendLine(SegmentIDBGN + '*' + this.referenceNumber + '*' + this.bGNDate + '*' + this.bGNTime + '*' + TimeCodeBGN + '*' +
                ReferenceIdentificationBGN06 + '*' + NotUsedBGN07 + '*' + ActionCode + SegmentTerminator);

            // Ref
            tempSB.AppendLine(SegmentIDRef + '*' + RefReferenceNumberQualifier + '*' + RefReferenceNumber + SegmentTerminator);

            // DTP
            tempSB.AppendLine(SegmentIDDTP + '*' + DateTimeQualifierDTP01 + '*' + DateTimePeriodFormatQualifierDTP02 + '*' +
                this.dateTimePeriodDTOP03 + SegmentTerminator);

            // N1A
            tempSB.AppendLine(N1SegmentID + '*' + EntityIdentCodeSponser + '*' + NameN1A + '*' +
                N1IdentificationCodeQualifier + '*' + IdentificationCodeN104 + SegmentTerminator);

            // N1B
            tempSB.AppendLine(N1SegmentID + '*' + N1BEntityIdentifierCode + '*' + N1BName + '*' +
                N1IdentificationCodeQualifier + '*' + ReceiverID + SegmentTerminator);

            // N1C
            tempSB.AppendLine(N1SegmentID + '*' + N1CEntityIdentifierCode + '*' + N1CName + '*' + N1IdentificationCodeQualifier + '*' +
                N1CIdentificationCode + SegmentTerminator);

            return tempSB.ToString();
        }
    }
}
