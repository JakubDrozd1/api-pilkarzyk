﻿using DataLibrary.Model.DTO;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.IRepository
{
    public interface ICreateTokensRepository
    {
        Task<GetTokenResponse> GenerateJwtTokenAsync(GetTokenRequest tokenRequest, FbTransaction? transaction = null);
    }
}