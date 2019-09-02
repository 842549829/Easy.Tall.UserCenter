using Easy.Tall.UserCenter.Entity.Extend;
using Easy.Tall.UserCenter.Entity.Model;

namespace Easy.Tall.UserCenter.Services.Factory
{
    /// <summary>
    /// 企业信息
    /// </summary>
    public static class EnterpriseFactory
    {
        /// <summary>
        /// 创建企业
        /// </summary>
        /// <param name="request">添加企业信息</param>
        /// <returns>角色</returns>
        public static Enterprise ToEnterprise(this EnterpriseAddRequest request)
        {
            return new Enterprise
            {
                Name = request.Name,
                Abbreviation = request.Abbreviation,
                Account = request.Account,
                Address = request.Address,
                ContactMobile = request.ContactMobile,
                Contacts = request.Contacts,
                Fax = request.Fax,
                Mail = request.Mail,
                OrganizingCode = request.OrganizingCode,
                ParentId = request.ParentId,
                Person = request.Person,
                Type = request.Type
            };
        }
    }
}