using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Easy.Tall.UserCenter.Framework.Constant;
using Easy.Tall.UserCenter.Framework.Encrypt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Easy.Tall.UserCenter.WebApi.Extensions
{
    /// <summary>
    /// JwtToken
    /// </summary>
    public class JwtTokenValidator : ISecurityTokenValidator
    {
        /// <summary>
        /// 配置信息
        /// </summary>
        private readonly SsoOptions _ssoOptions;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ssoOptions">配置信息</param>
        public JwtTokenValidator(IOptions<SsoOptions> ssoOptions)
        {
            _ssoOptions = ssoOptions.Value;
        }

        /// <summary>
        /// Returns true if the token can be read, false otherwise.
        /// </summary>
        public bool CanReadToken(string securityToken)
        {
            var claimsPrincipal = ValidateToken(securityToken, out _);
            if (securityToken == null || claimsPrincipal == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Validates a token passed as a string using <see cref="T:Microsoft.IdentityModel.Tokens.TokenValidationParameters" />
        /// </summary>
        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            return ValidateToken(securityToken, out validatedToken);
        }

        /// <summary>
        /// 如果可以验证令牌，则返回true
        /// </summary>
        public bool CanValidateToken => true;

        /// <summary>
        /// 获取并设置将处理的最大大小(以字节为单位)
        /// </summary>
        public int MaximumTokenSizeInBytes { get; set; }

        /// <summary>
        /// 创建Token
        /// </summary>
        /// <param name="userId">userId</param>
        /// <returns>string</returns>
        public string GenerateToken(string userId)
        {
            var exp = ((DateTime.Now.AddMinutes(_ssoOptions.Expire).ToUniversalTime().Ticks - 621355968000000000) / 10000000).ToString();
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Iss, _ssoOptions.Issuer),
                new Claim(JwtRegisteredClaimNames.Aud, _ssoOptions.Audience),
                new Claim(JwtRegisteredClaimNames.Exp, exp),
                new Claim(AppSettingsSection.Uid, userId)
            };
            var jwtHeader = new JwtHeader(new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_ssoOptions.Secret)), SecurityAlgorithms.HmacSha256));
            var jwtPayload = new JwtPayload(claims);
            var token = new JwtSecurityToken(jwtHeader, jwtPayload);
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            return jwtToken;
        }

        /// <summary>
        /// 验证token
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="securityToken">securityToken</param>
        /// <returns>ClaimsPrincipal</returns>
        private ClaimsPrincipal ValidateToken(string token, out SecurityToken securityToken)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                // 验证签名
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_ssoOptions.Secret)),
                //验证发行者
                ValidateIssuer = true,
                ValidIssuer = _ssoOptions.Issuer,
                //验证接受者
                ValidateAudience = true,
                ValidAudience = _ssoOptions.Audience,
                //验证token有效期
                ValidateLifetime = true
            };
            var handler = new JwtSecurityTokenHandler();
            return handler.ValidateToken(token, tokenValidationParameters, out securityToken);
        }
    }
}