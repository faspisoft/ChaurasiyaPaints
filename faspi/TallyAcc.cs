using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;


namespace faspi
{
    public class ENVELOPE
    {
        public clsHEADER HEADER { get; set; }
        public clsBODY BODY { get; set; }
    }
    public class clsHEADER
    {
        public string TALLYREQUEST { get; set; }

    }
    public class clsBODY
    {
        public clsIMPORTDATA IMPORTDATA { get; set; }

    }
    public class clsIMPORTDATA
    {
        public clsREQUESTDESC REQUESTDESC { get; set; }
        [XmlArrayItem("TALLYMESSAGE")]
        public List<clsTALLYMESSAGE> REQUESTDATA { get; set; }

    }
    public class clsREQUESTDESC
    {
        public string REPORTNAME { get; set; }

    }

    public class clsTALLYMESSAGE
    {
        public clsLEDGER LEDGER { get; set; }
        public clsGROUP GROUP { get; set; }
        public clsVOUCHER VOUCHER { get; set; }
    }

    public class clsLEDGER
    {
        [XmlAttribute]
        public string NAME { get; set; }
        [XmlAttribute]
        public string ACTION { get; set; }
        [XmlElement("NAME.LIST")]
        public clsNAMELIST NAME_LIST { get; set; }

        public string GSTREGISTRATIONTYPE { get; set; }
        public string PARENT { get; set; }
        public string PARTYGSTIN { get; set; }
        public string LEDSTATENAME { get; set; }
        public string ISBILLWISEON { get; set; }
        public string AFFECTSSTOCK { get; set; }
        public string PINCODE { get; set; }
        public double OPENINGBALANCE { get; set; }
        public string USEFORVAT { get; set; }
        public string TAXCLASSIFICATIONNAME { get; set; }
        public string TAXTYPE { get; set; }
        public string RATEOFTAXCALCULATION { get; set; }
    }
    public class clsNAMELIST
    {
        public string NAME { get; set; }

    }
    public class clsGROUP
    {
        [XmlAttribute]
        public string RESERVEDNAME { get; set; }
        [XmlAttribute]
        public string NAME { get; set; }


        public string PARENT { get; set; }
        public string ISSUBLEDGER { get; set; }
        public string ISBILLWISEON { get; set; }
        public string ISCOSTCENTRESON { get; set; }
        [XmlElement("LANGUAGENAME.LIST")]
        public clsLANGUAGENAMELIST LANGUAGENAME_LIST { get; set; }

    }
    public class clsLANGUAGENAMELIST
    {
        [XmlElement("NAME.LIST")]
        public clsNAMELISTGrp NAME_LIST { get; set; }
    }
    public class clsNAMELISTGrp
    {
        [XmlAttribute]
        public string TYPE { get; set; }

        public string NAME { get; set; }

    }


    public class clsVOUCHER
    {
        [XmlAttribute]
        public string ACTION { get; set; }
        [XmlAttribute]
        public string VCHTYPE { get; set; }


        public string DATE { get; set; }
        public string REFERENCEDATE { get; set; }
        public string NARRATION { get; set; }
        public string VOUCHERTYPENAME { get; set; }
        public string REFERENCE { get; set; }
        public string VOUCHERNUMBER { get; set; }

        public string PARTYLEDGERNAME { get; set; }
        public string EFFECTIVEDATE { get; set; }
        [XmlElement("ALLLEDGERENTRIES.LIST")]
        public List<clsALLLEDGERENTRIESLIST> ALLLEDGERENTRIES_LIST { get; set; }

    }
    public class clsALLLEDGERENTRIESLIST
    {
        public string LEDGERNAME { get; set; }

        public string ISDEEMEDPOSITIVE { get; set; }
        public string ISLASTDEEMEDPOSITIVE { get; set; }

        public double AMOUNT { get; set; }
    }
}
