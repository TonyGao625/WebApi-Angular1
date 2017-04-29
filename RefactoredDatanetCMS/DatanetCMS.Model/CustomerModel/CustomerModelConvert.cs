using DatanetCMS.DAO;
using DatanetCMS.Model.BuyerGroupModel;
using DatanetCMS.Model.ManagerModel;

namespace DatanetCMS.Model.CustomerModel
{
    public static class CustomerModelConvert
    {
        public static CustomerModel ToCustomerModel(this Customer customer)
        {
            var model = new CustomerModel()
            {
                Id = customer.Id,
                CompanyName = customer.CompanyName,
                LoginId = customer.LoginId,
                LoginPassword = customer.LoginPassword,
                ContactName = customer.ContactName,
                ContactPhone = customer.ContactPhone,
                ContactEmail = customer.ContactEmail,
                ManagerId = customer.ManagerId,
                ContractNo = customer.ContractNo,
                DisplayContractNo = customer.DisplayContractNo,
                PurchaseType = customer.PurchaseType,
                PoDocMandatory = customer.PoDocMandatory,
                BuyerGroupId = customer.BuyerGroupId,
                DeliveryCharge = customer.DeliveryCharge,
                Logo = customer.Logo,
            };
            if (customer.BuyerGroupId != 0 && customer.BuyerGroup != null)
            {
                model.BuyerGroup = customer.BuyerGroup.ToBuyerGroupModel();
            }
            if (customer.ManagerId != 0 && customer.ManagerId!=null && customer.Manager != null)
            {
                model.Manager = customer.Manager.ToManagerModel();
            }
            return model;
        }

        public static CustomerModel ToCustomerModelWithourLogo(this Customer customer)
        {
            var model = new CustomerModel()
            {
                Id = customer.Id,
                CompanyName = customer.CompanyName,
                ContactName = customer.ContactName,
                ContactPhone = customer.ContactPhone,
                ContactEmail = customer.ContactEmail,
                ManagerId = customer.ManagerId,
                BuyerGroupId = customer.BuyerGroupId,
            };
            if (model.BuyerGroupId != 0)
            {
                model.BuyerGroup = customer.BuyerGroup.ToBuyerGroupModel();
            }
            if (model.ManagerId != 0 && model.ManagerId != null)
            {
                model.Manager = customer.Manager.ToManagerModel();
            }
            return model;
        }
        public static Customer ToCustomerModel(this CustomerModel model)
        {
            var customer = new Customer()
            {
                Id = model.Id,
                CompanyName = model.CompanyName,
                LoginId = model.LoginId,
                LoginPassword = model.LoginPassword,
                //ContactFirstName = model.ContactFirstName,
                //ContactLastName = model.ContactLastName,
                ContactName = model.ContactName,
                ContactPhone = model.ContactPhone,
                ContactEmail = model.ContactEmail,
                ManagerId = model.ManagerId,
                ContractNo = model.ContractNo,
                DisplayContractNo = model.DisplayContractNo,
                //PurchaseOnContract = model.PurchaseOnContract,
                //PoMandatory = model.PoMandatory,
                PurchaseType = model.PurchaseType,
                PoDocMandatory = model.PoDocMandatory,
                BuyerGroupId = model.BuyerGroupId,
                DeliveryCharge = model.DeliveryCharge,
                Logo = model.Logo,
            };
            return customer;
        }

        public static CustomerModel ToCustomerModelSimple(this Customer customer)
        {
            var model = new CustomerModel()
            {
                Id = customer.Id,
                CompanyName = customer.CompanyName,
                LoginId = customer.LoginId,
                LoginPassword = customer.LoginPassword,
                ContactName = customer.ContactName,
                ContactPhone = customer.ContactPhone,
                ContactEmail = customer.ContactEmail,
                ManagerId = customer.ManagerId,
                ContractNo = customer.ContractNo,
                DisplayContractNo = customer.DisplayContractNo,
                PurchaseType = customer.PurchaseType,
                PoDocMandatory = customer.PoDocMandatory,
                BuyerGroupId = customer.BuyerGroupId,
                DeliveryCharge = customer.DeliveryCharge,
                Logo = customer.Logo,
            };
            return model;
        }

        public static CustomerModel ToCustomerModelForLogin(this Customer customer)
        {
            var model = new CustomerModel()
            {
                Id = customer.Id,
                CompanyName = customer.CompanyName,
                LoginId = customer.LoginId,
                LoginPassword = customer.LoginPassword,
                ContactName = customer.ContactName,
                ContactPhone = customer.ContactPhone,
                ContactEmail = customer.ContactEmail,
                ManagerId = customer.ManagerId,
                ContractNo = customer.ContractNo,
                DisplayContractNo = customer.DisplayContractNo,
                PurchaseType = customer.PurchaseType,
                PoDocMandatory = customer.PoDocMandatory,
                BuyerGroupId = customer.BuyerGroupId,
                DeliveryCharge = customer.DeliveryCharge,
                Logo = customer.Logo,
            };
            if(model.ManagerId != 0)
            {
                model.Manager = customer.Manager.ToManagerModel();
            }
            return model;
        }
    }
}
