using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace faspi
{
     [DataContract]
    class clsPLEwayBillList
    {
        [DataMember]
        public string version { get; set; }
        [DataMember]
        public List<clsPLEwayBill> billLists { get; set; }

    }

     [DataContract]
     class clsPLEwayBill
     {
         [DataMember]
         public string userGstin { get; set; }
         [DataMember]
         public string supplyType { get; set; }
         [DataMember]
         public int subSupplyType { get; set; }
         [DataMember]
         public string docType { get; set; }
         [DataMember]
         public string docNo { get; set; }
         [DataMember]
         public string docDate { get; set; }
         [DataMember]
         public string fromTrdName { get; set; }
         [DataMember]
         public string fromGstin { get; set; }
         [DataMember]
         public string fromAddr1 { get; set; }
         [DataMember]
         public string fromAddr2 { get; set; }
         [DataMember]
         public string fromPlace { get; set; }
         [DataMember]
         public Int64 fromPincode { get; set; }
         [DataMember]
         public int fromStateCode { get; set; }
         [DataMember]
         public int actualFromStateCode { get; set; }
         [DataMember]
         public string toTrdName { get; set; }
         [DataMember]
         public string toGstin { get; set; }
         [DataMember]
         public string toAddr1 { get; set; }
         [DataMember]
         public string toAddr2 { get; set; }
         [DataMember]
         public string toPlace { get; set; }
         [DataMember]
         public Int64 toPincode { get; set; }
         [DataMember]
         public int toStateCode { get; set; }
         [DataMember]
         public int actualToStatecode { get; set; }
         [DataMember]
         public double totalValue { get; set; }
         [DataMember]
         public double cgstValue { get; set; }
         [DataMember]
         public double sgstValue { get; set; }
         [DataMember]
         public double igstValue { get; set; }
         [DataMember]
         public double cessValue { get; set; }
         [DataMember]
         public Int64 mainHsnCode { get; set; }

         [DataMember]
         public double totInvValue { get; set; }
         [DataMember]
         public int transMode { get; set; }
         [DataMember]
         public int transType { get; set; }
         [DataMember]
         public long transDistance { get; set; }
         [DataMember]
         public string transporterName { get; set; }
         [DataMember]
         public string transporterId { get; set; }
         [DataMember]
         public string transDocNo { get; set; }
         [DataMember]
         public string transDocDate { get; set; }
         [DataMember]
         public string vehicleNo { get; set; }
         [DataMember]
         public string vehicleType { get; set; }
         [DataMember]
         public double TotNonAdvolVal { get; set; }
         [DataMember]
         public double OthValue { get; set; }

         [DataMember]
         public List<clsPLEwayBillItem> itemList { get; set; }
     }

     [DataContract]
     class clsPLEwayBillItem
     {
         [DataMember]
         public int itemNo { get; set; }
         [DataMember]
         public string productName { get; set; }
         [DataMember]
         public string productDesc { get; set; }
         [DataMember]
         public Int64 hsnCode { get; set; }
         [DataMember]
         public double quantity { get; set; }
         [DataMember]
         public string qtyUnit { get; set; }
         [DataMember]
         public double taxableAmount { get; set; }
         [DataMember]
         public double sgstRate { get; set; }
         [DataMember]
         public double igstRate { get; set; }
         [DataMember]
         public double cgstRate { get; set; }
         [DataMember]
         public double cessRate { get; set; }
         [DataMember]
         public double cessNonAdvol { get; set; }
     }

}
