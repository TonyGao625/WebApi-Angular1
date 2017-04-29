
using System;
using System.Threading.Tasks;
using DatanetCMS.Common;
using DatanetCMS.Common.Log;
using DatanetCMS.Model;
using DatanetCMS.Model.AccountModel;
using DatanetCMS.Repository;
using DatanetCMS.Model.CustomerModel;

namespace DatanetCMS.Service
{
    public class AccountService
    {
        private readonly CustomerRepository _customerRepository;
        private readonly ManagerRepository _managerRepository;
        public AccountService()
        {
            _customerRepository = new CustomerRepository();
            _managerRepository = new ManagerRepository();
        }

        public async Task<ViewResult<LoginModel>> Login(string userName, string userPwd)
        {
            var result = new ViewResult<LoginModel>();
            var loginModel = new LoginModel();
            try
            {
                var customer = await _customerRepository.GetByName(userName);
                var manager = await _managerRepository.GetByName(userName);

                if (customer != null)
                {
                    if (customer.LoginPassword == userPwd)
                    {
                        loginModel.Id = customer.Id;
                        loginModel.Logo = customer.Logo;
                        loginModel.Name = customer.LoginId;
                        loginModel.Role = Constants.RoleCustomer;
                        loginModel.Customer = customer.ToCustomerModelForLogin();
                        result.Data = loginModel;
                        return result;
                    }
                    result.Status = -3;
                    result.Message = "The username and password you entered did not match our record. Please double-check and try again.";                    
                    return result;
                }
                if (manager != null)
                {
                    if (manager.Password == userPwd)
                    {
                        loginModel.Id = manager.Id;
                        loginModel.Name = manager.LoginName;
                        loginModel.Role = manager.Role;
                        result.Data = loginModel;
                        
                        return result;
                    }
                    result.Status = -3;
                    result.Message = "The username and password you entered did not match our record. Please double-check and try again.";
                    return result;
                }
                result.Status = -2;
                result.Message = "The username and password you entered did not match our record. Please double-check and try again.";
            }
            catch (Exception ex)
            {
                result.Status = -1;
                result.Message = ex.Message;
                Logger.WriteErrorLog("AccountService", "Login", ex);
            }
            return result;
        }
    }
}
