// <copyright file="Header.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using EC_to_VSP_EDI;

public class Header {
    private const string SegmentID = "ISA";
    private const string AuthorizationInfoQualifier = "00"; // ISA01
    private const string AuthorizationInfo = "          "; // ISA02
    private const string SecurityInfoQualifier = "00"; // ISA03
    private const string SecurityInfo = "          "; // ISA04
    private const string SenderIDQualifier = "ZZ"; // ISA05
    private const string SenderID = "DDCA07012      "; // ISA06
    private const string ReceiverIDQualifier = "ZZ";
    private const string ReceiverID = "942411167      ";
    private readonly string interchangeDate;
    private readonly string interchangeTime;
    private const char InterchangeControlID = '^';
    private const string InterchangeControlVersionNum = "00501";
    private readonly string interchangeControlNum;
    private const char AcknowledgementRequested = '0';
    private readonly char usageIndicator;
    private const char ComponentElementSeparator = ':';
    private const char SegmentTerminator = '~';

    public Header() {
        this.interchangeDate = InterchangeTracker.GetInterchangeDate();
        this.interchangeTime = InterchangeTracker.GetInterchangeTime();
        this.interchangeControlNum = InterchangeTracker.GetInterchangeNumber().ToString().PadLeft(9, '0');

        if (Form1.EnrollType == TransactionSetPurposes.Test) {
            this.usageIndicator = 'T';
        } else {
            this.usageIndicator = 'P';
        }
    }

    public new string ToString() {
        return SegmentID + '*' + AuthorizationInfoQualifier + '*' + AuthorizationInfo + '*' + SecurityInfoQualifier + '*' +
            SecurityInfo + '*' + SenderIDQualifier + '*' + SenderID + '*' + ReceiverIDQualifier + '*' + ReceiverID + '*' + this.interchangeDate + '*' +
            this.interchangeTime + '*' + InterchangeControlID + '*' + InterchangeControlVersionNum + '*' + this.interchangeControlNum + '*' +
            AcknowledgementRequested + '*' + this.usageIndicator + '*' + ComponentElementSeparator + SegmentTerminator;
    }
}
