// <copyright file="Trailer.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EC_to_VSP_EDI {
    public class Trailer {
        private const string SegmentIDSE = "SE";
        private readonly string numberOfIncludedSegmentsSE01;
        private readonly string transactionSetControlNumberSE02;
        private const string SegmentIDGE = "GE";
        private const string NumberOfTransactionSetsIncludedGE01 = "1";
        private readonly string groupControlNumberGE02;
        private const string SegmentIDIEA = "IEA";
        private const string NumberOfFunctionalGroupsIncludedIEA01 = "1";
        private readonly string interchangeControlNumberIEA02;
        private const string SegmentTerminator = "~";

        public Trailer() {
            this.numberOfIncludedSegmentsSE01 = Form1.Enrollments.Count.ToString();
            this.transactionSetControlNumberSE02 = InterchangeTracker.GetInterchangeNumber().ToString();
            this.groupControlNumberGE02 = this.transactionSetControlNumberSE02.PadLeft(4, '0');
            this.interchangeControlNumberIEA02 = this.transactionSetControlNumberSE02;
        }

        public new string ToString() {
            return SegmentIDSE + '*' + this.numberOfIncludedSegmentsSE01 + '*' + this.transactionSetControlNumberSE02.PadLeft(4, '0') + SegmentTerminator + '\n' +
                SegmentIDGE + '*' + NumberOfTransactionSetsIncludedGE01 + '*' + this.groupControlNumberGE02 + SegmentTerminator + '\n' +
                SegmentIDIEA + '*' + NumberOfFunctionalGroupsIncludedIEA01 + '*' + this.interchangeControlNumberIEA02.PadLeft(9, '0') + SegmentTerminator;
        }
    }
}
