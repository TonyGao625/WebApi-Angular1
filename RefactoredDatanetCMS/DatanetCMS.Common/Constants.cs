using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatanetCMS.Common
{
    public static class Constants
    {
        public enum OrderMode
        {
            Quote = 9,
            Order = 7,
            Deleted= 0
        }

        public enum PurchaseType
        {
            ContractNumber = 1,
            PoNumber = 2
        }

        public static string OrderHistoryAddOperator = "Add";
        public static string OrderHistoryErrorOperator = "Error";

        public static string OrderHistoryAddQuote= "Add a new quote";
        public static string OrderHistoryAddOrder = "Add a new order";
        public static string OrderHistoryQuoteToOrder = "Add quote to order";

        public static string OrderHistoryUpdateOperator = "Update";
        public static string OrderHistoryDeleteOperator = "Delete";

        public static string ErrorOrderHasNoProduct = "Order has no product";

        public static string RoleCustomer = "Customer";
    }
}
