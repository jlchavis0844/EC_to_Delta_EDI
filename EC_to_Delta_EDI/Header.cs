using EC_to_VSP_EDI;

public class Header
{
    private const string SegmentID                       = "ISA";
    private const string AuthorizationInfoQualifier      = "00";//ISA01
    private const string AuthorizationInfo               = "          ";//ISA02
    private const string SecurityInfoQualifier           = "00";//ISA03
    private const string SecurityInfo                    = "          ";//ISA04
    private const string SenderIDQualifier               = "ZZ";//ISA05
    private const string SenderID                        = "DDPPGGGGG      ";//ISA06
    private const string ReceiverIDQualifier             = "ZZ";
    private const string ReceiverID                      = "942411167      ";
    private string       InterchangeDate;
    private string       InterchangeTime;
    private const char   InterchangeControlID            = '^';
    private const string InterchangeControlVersionNum    = "00501";
    private string       InterchangeControlNum;
    private const char   AcknowledgementRequested        = '0';
    private char         UsageIndicator;
    private const char   ComponentElementSeparator       = ':';
    private const char   SegmentTerminator               = '~';

    public Header(){
        InterchangeDate = InterchangeTracker.GetInterchangeDate();
        InterchangeTime = InterchangeTracker.GetInterchangeTime();
        InterchangeControlNum = InterchangeTracker.GetInterchangeNumber().ToString().PadLeft(9,'0');
        
        if(Form1.enrollType == TransactionSetPurposes.Test) {
            UsageIndicator = 'T';
        } else {
            UsageIndicator = 'P';
        }
	}

    public new string ToString() {
        return SegmentID + '*' + AuthorizationInfoQualifier + '*' + AuthorizationInfo + '*' + SecurityInfoQualifier + '*' +
            SecurityInfo + '*' + SenderIDQualifier + '*' + SenderID + '*' + ReceiverIDQualifier + '*' + ReceiverID + '*' + InterchangeDate + '*' +
            InterchangeTime + '*' + InterchangeControlID + '*' + InterchangeControlVersionNum + '*' + InterchangeControlNum + '*' +
            AcknowledgementRequested + '*' + UsageIndicator + '*' + ComponentElementSeparator + SegmentTerminator;
    }
}
