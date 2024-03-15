using System.Data;
using Dapper;
using DataLibrary.Entities;
using DataLibrary.Helper;
using DataLibrary.IRepository.Meetings;
using DataLibrary.Model.DTO.Request.TableRequest;
using FirebirdSql.Data.FirebirdClient;

namespace DataLibrary.Repository.Meetings
{
    public class UpdateMeetingsRepository(FbConnection dbConnection, FbTransaction? fbTransaction) : IUpdateMeetingsRepository
    {
        private readonly FbConnection _dbConnection = dbConnection;
        private readonly FbTransaction? _fbTransaction = fbTransaction;

        public async Task UpdateMeetingAsync(MEETINGS meeting)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                var updateBuilder = new QueryBuilder<MEETINGS>()
                    .Update("MEETINGS ", meeting)
                    .Where("ID_MEETING = @ID_MEETING ");
                string updateQuery = updateBuilder.Build();
                await _dbConnection.ExecuteAsync(updateQuery, meeting, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }

        public async Task UpdateColumnMeetingAsync(GetUpdateMeetingRequest getUpdateMeetingRequest, int meetingId)
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                await _dbConnection.OpenAsync();
            }
            try
            {
                DynamicParameters dynamicParameters = new();
                var updateBuilder = new QueryBuilder<GetUpdateMeetingRequest>()
                    .UpdateColumns("MEETINGS", getUpdateMeetingRequest.Column)
                    .Where("ID_MEETING = @MeetingId");
                string updateQuery = updateBuilder.Build();
                dynamicParameters.Add("@MeetingId", meetingId);

                foreach (string column in getUpdateMeetingRequest.Column)
                {
                    switch (column)
                    {
                        case "DATE_MEETING":
                            {
                                DateTime dateMeeting = getUpdateMeetingRequest.DATE_MEETING ?? throw new Exception("Date is null");
                                dynamicParameters.Add($"@{column}", dateMeeting);
                            }
                            break;
                        case "PLACE":
                            {
                                string place = getUpdateMeetingRequest.PLACE ?? throw new Exception("Place is null");
                                dynamicParameters.Add($"@{column}", place);
                            }
                            break;
                        case "QUANTITY":
                            {
                                int quantity = getUpdateMeetingRequest.QUANTITY ?? throw new Exception("Quantity is null");
                                dynamicParameters.Add($"@{column}", quantity);
                            }
                            break;
                        case "DESCRIPTION":
                            {
                                string? description = getUpdateMeetingRequest.DESCRIPTION;
                                dynamicParameters.Add($"@{column}", description);
                            }
                            break;
                    }

                }
                await _dbConnection.ExecuteAsync(updateQuery, dynamicParameters, _fbTransaction);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
    }
}
