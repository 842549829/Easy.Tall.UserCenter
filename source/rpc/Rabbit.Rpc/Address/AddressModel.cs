using System.Net;

namespace Rabbit.Rpc.Address
{
    /// <summary>
    /// 一个抽象到地址模型
    /// </summary>
    public abstract class AddressModel
    {
        /// <summary>
        /// 地址权重
        /// </summary>
        public Weights Weights { get; set; }

        /// <summary>
        /// 创建终结点。
        /// </summary>
        /// <returns>终结点</returns>
        public abstract EndPoint CreateEndPoint();

        /// <summary>
        /// 重写后的标识。
        /// </summary>
        /// <returns>一个字符串。</returns>
        public abstract override string ToString();

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            var model = obj as AddressModel;
            if (model == null)
            {
                return false;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return model.ToString() == ToString();
        }

        /// <summary>Serves as the default hash function. </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// 相等比较器
        /// </summary>
        /// <param name="model1">抽象的地址模型1</param>
        /// <param name="model2">抽象的地址模型2</param>
        /// <returns>比较结果</returns>
        public static bool operator ==(AddressModel model1, AddressModel model2)
        {
            return Equals(model1, model2);
        }

        /// <summary>
        /// 不相等比较器
        /// </summary>
        /// <param name="model1">抽象的地址模型1</param>
        /// <param name="model2">抽象的地址模型2</param>
        /// <returns>比较结果</returns>
        public static bool operator !=(AddressModel model1, AddressModel model2)
        {
            return !Equals(model1, model2);
        }
    }
}