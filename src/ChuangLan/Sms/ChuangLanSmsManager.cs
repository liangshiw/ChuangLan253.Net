﻿using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ChuangLan.Client;
using ChuangLan.Client.ApiRequest;
using ChuangLan.Client.ApiResult;
using ChuangLan.Configuration;
using Newtonsoft.Json;

namespace ChuangLan.Sms
{
    public class ChuangLanSmsManager : ChuangLanManagerBase, IChuangLanSmsManager
    {

        public ChuangLanSmsManager(ChuangLanOptions chuangLanOptions)
            : base(chuangLanOptions)
        {
        }

        public async Task<ApiSendSmsResultBase> SendAsync(SingleSms sms)
        {
            var url = SendCore(sms);
            return await ApiHttpClient.PostAsync<ApiSendSmsResultBase>(url, JsonConvert.SerializeObject(sms));

        }

        private string SendCore(SingleSms sms)
        {
            string url;
            CheckSmsModel(sms);

            if (Regex.IsMatch(sms.Msg, VariableRegex))
            {
                url = ApiUrl(ApiConsts.VariableSmsUrl);
                Check.CheckNullOrWhiteSpace(sms.Params, nameof(sms.Params));
            }
            else
            {
                url = ApiUrl(ApiConsts.SimpleSmsUrl);
                Check.CheckNullOrWhiteSpace(sms.Phone, nameof(sms.Phone));
            }

            return url;
        }

        public ApiSendSmsResultBase Send(SingleSms sms)
        {
            return ApiHttpClient.Post<ApiSendSmsResultBase>(SendCore(sms), JsonConvert.SerializeObject(sms));
        }

        public async Task<ApiBatchSmsResult> BatchSendAsync(BatchSms sms)
        {
            return await ApiHttpClient.PostAsync<ApiBatchSmsResult>(BatchSendCore(sms), JsonConvert.SerializeObject(sms));
        }

        private string BatchSendCore(BatchSms sms)
        {
            CheckSmsModel(sms);
            string url;
            if (Regex.IsMatch(sms.Msg, VariableRegex))
            {
                url = ApiUrl(ApiConsts.VariableSmsUrl);
                Check.CheckNullOrWhiteSpace(sms.Params, nameof(sms.Params));
            }
            else
            {
                url = ApiUrl(ApiConsts.SimpleSmsUrl);
                Check.CheckNull(sms.Phone, nameof(sms.Phone));
                foreach (var phone in sms.Phone)
                {
                    Check.CheckNullOrWhiteSpace(phone, nameof(phone));
                }
            }

            return url;
        }

        public ApiBatchSmsResult BatchSend(BatchSms sms)
        {
            return ApiHttpClient.Post<ApiBatchSmsResult>(BatchSendCore(sms), JsonConvert.SerializeObject(sms));
        }

        public async Task<ApiBalanceResult> BalanceAsync()
        {
            return await ApiHttpClient.PostAsync<ApiBalanceResult>(ApiUrl(ApiConsts.BalanceUrl), JsonConvert.SerializeObject(new { ChuangLanOptions.Account, ChuangLanOptions.Password }));
        }

        public ApiBalanceResult Balance()
        {
            return  ApiHttpClient.Post<ApiBalanceResult>(ApiUrl(ApiConsts.BalanceUrl), JsonConvert.SerializeObject(new { ChuangLanOptions.Account, ChuangLanOptions.Password }));
        }

        public async Task<ApiPullMoResult> PullMoAsync(ApiPullMo input)
        {
            input.Account = ChuangLanOptions.Account;
            input.Password = ChuangLanOptions.Password;
            return await ApiHttpClient.PostAsync<ApiPullMoResult>(ApiUrl(ApiConsts.BalanceUrl), JsonConvert.SerializeObject(input));
        }

        public ApiPullMoResult PullMo(ApiPullMo input)
        {
            input.Account = ChuangLanOptions.Account;
            input.Password = ChuangLanOptions.Password;
            return  ApiHttpClient.Post<ApiPullMoResult>(ApiUrl(ApiConsts.BalanceUrl), JsonConvert.SerializeObject(input));
        }

        public async Task<ApiPullReportResult> PullReportAsync(ApiPullReport input)
        {
            input.Account = ChuangLanOptions.Account;
            input.Password = ChuangLanOptions.Password;
            return await ApiHttpClient.PostAsync<ApiPullReportResult>(ApiUrl(ApiConsts.BalanceUrl), JsonConvert.SerializeObject(input));
        }

        public ApiPullReportResult PullReport(ApiPullReport input)
        {
            input.Account = ChuangLanOptions.Account;
            input.Password = ChuangLanOptions.Password;
            return  ApiHttpClient.Post<ApiPullReportResult>(ApiUrl(ApiConsts.BalanceUrl), JsonConvert.SerializeObject(input));
        }

        private void CheckSmsModel(SmsModelBase sms)
        {
            Check.CheckNull(sms, nameof(sms));
            Check.CheckNullOrWhiteSpace(sms.Msg, nameof(sms.Msg));
            if (!sms.Report.HasValue)
            {
                sms.Report = ChuangLanOptions.Report;
            }

            sms.Msg = ChuangLanOptions.SignName + sms.Msg;
            sms.Account = ChuangLanOptions.Account;
            sms.Password = ChuangLanOptions.Password;
        }


    }
}
