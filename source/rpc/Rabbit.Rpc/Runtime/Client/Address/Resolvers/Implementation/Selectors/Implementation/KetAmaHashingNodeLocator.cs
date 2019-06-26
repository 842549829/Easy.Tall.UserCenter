using System.Collections.Generic;
using System.Linq;

namespace Rabbit.Rpc.Runtime.Client.Address.Resolvers.Implementation.Selectors.Implementation
{
    /// <summary>
    /// KetAmaHashingNodeLocator
    /// </summary>
    public class KetAmaHashingNodeLocator
    {
        /// <summary>
        /// ketAmaNodes
        /// </summary>
        private readonly SortedList<long, string> _ketAmaNodes;

        /// <summary>
        /// numReps
        /// </summary>
        private const int _numReps = 160;

        /// <summary>
        /// KetAmaHashingNodeLocator
        /// </summary>
        /// <param name="nodes">nodes</param>
        public KetAmaHashingNodeLocator(IEnumerable<string> nodes)
        {
            _ketAmaNodes = new SortedList<long, string>();
            //numReps = nodeCopies;
            //对所有节点，生成nCopies个虚拟结点
            foreach (var node in nodes)
            {
                //每四个虚拟结点为一组
                for (var i = 0; i < _numReps / 4; i++)
                {
                    //getKeyForNode方法为这组虚拟结点得到惟一名称 
                    var digest = HashAlgorithm.ComputeMd5(node + i);
                    /** Md5是一个16字节长度的数组，将16字节的数组每四个字节一组，分别对应一个虚拟结点，这就是为什么上面把虚拟结点四个划分一组的原因*/
                    for (var h = 0; h < 4; h++)
                    {
                        var m = HashAlgorithm.Hash(digest, h);
                        _ketAmaNodes[m] = node;
                    }
                }
            }
        }

        /// <summary>
        /// GetPrimary
        /// </summary>
        /// <param name="k">k</param>
        /// <returns>string</returns>
        public string GetPrimary(string k)
        {
            var digest = HashAlgorithm.ComputeMd5(k);
            var rv = GetNodeForKey(HashAlgorithm.Hash(digest, 0));
            return rv;
        }

        /// <summary>
        /// GetNodeForKey
        /// </summary>
        /// <param name="hash">hash</param>
        /// <returns>string</returns>
        private string GetNodeForKey(long hash)
        {
            var key = hash;
            //如果找到这个节点，直接取节点，返回   
            if (!_ketAmaNodes.ContainsKey(key))
            {
                var tailMap = (from coll in _ketAmaNodes
                    where coll.Key > hash
                    select new { coll.Key }).ToList();
                key = !tailMap.Any() ? _ketAmaNodes.FirstOrDefault().Key : tailMap.First().Key;
            }
            var rv = _ketAmaNodes[key];
            return rv;
        }
    }
}