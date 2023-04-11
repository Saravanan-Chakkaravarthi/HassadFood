using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HassadFood.Interface;
using HassadFood.MVVMHelper;
using HassadFood.WebService;
using Newtonsoft.Json;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(APIClient))]
namespace HassadFood.WebService
{
    public class APIClient : BaseViewModel, IUserDetails
    {
        public APIClient()
        {
        }

        public async Task<bool> EMailVerification(string email)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/generate_mobile_pin/" + email)).Content.ReadAsStringAsync().Result;
            DataInfo.EmailPINVerifyInformation = JsonConvert.DeserializeObject<EmailPINVerify>(result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> PINVerification(string pin)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/validate_pin/" + Application.Current.Properties["OracleUserName"] as string + "/" + pin + "/" + DependencyService.Get<INotification>().DeviceID())).Content.ReadAsStringAsync().Result;
            DataInfo.EmailPINVerifyInformation = JsonConvert.DeserializeObject<EmailPINVerify>(result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> ResendVerficationPIN()
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/generate_mobile_pin/it@hassad.com/" + Application.Current.Properties["OracleUserName"] as string + "/" + DependencyService.Get<INotification>().DeviceID())).Content.ReadAsStringAsync().Result;
            DataInfo.EmailPINVerifyInformation = JsonConvert.DeserializeObject<EmailPINVerify>(result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> Authenticate(string username, string password)
        {
            byte[] b = Encoding.UTF8.GetBytes(password);
            string b64pass = "MGF8" + System.Convert.ToBase64String(b) + "1SBR";
            password = b64pass;
            b = Encoding.UTF8.GetBytes(password);
            b64pass = System.Convert.ToBase64String(b);
            password = b64pass;

            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var temp = DependencyService.Get<INotification>().DeviceID();
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_user_auth_p/" + username + "/" + password + "/" + DependencyService.Get<INotification>().DeviceID())).Content.ReadAsStringAsync().Result;
            //var result = (await client.GetAsync("http://10.165.10.25:8080/ords/apps/wshr/test")).Content.ReadAsStringAsync().Result;
            DataInfo.UserInformation = JsonConvert.DeserializeObject<LoginResponse>(result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> UserLogout()
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_user_log_out/" + Application.Current.Properties["OracleUserName"] as string + "/" + DependencyService.Get<INotification>().DeviceID() + "/" + DataInfo.UserInformation.hf_user_auth[0].sqno)).Content.ReadAsStringAsync().Result;
            //var result = (await client.GetAsync("http://10.165.10.25:8080/ords/apps/wshr/test")).Content.ReadAsStringAsync().Result;
            DataInfo.LogoutInformation = JsonConvert.DeserializeObject<LogoutResponse>(result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> AddDevice(PushNotificationRequest request)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var temp = JsonConvert.SerializeObject(request);
            var content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");
            var result = (await client.PostAsync(Constants.Oracle_URL + "wshr/add_device", content)).Content.ReadAsStringAsync().Result;
            DataInfo.AddDeviceInformation = JsonConvert.DeserializeObject<AddDeviceResponse>(result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> DeleteDevice()
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/delete_device/" + Application.Current.Properties["OracleUserName"] as string + "/" + DependencyService.Get<INotification>().DeviceID())).Content.ReadAsStringAsync().Result;
            //var result = (await client.GetAsync("http://10.165.10.25:8080/ords/apps/wshr/test")).Content.ReadAsStringAsync().Result;
            DataInfo.DeleteDeviceInformation = JsonConvert.DeserializeObject<DeleteDeviceResponse>(result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> GetWorkList()
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();   
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_work_list/" + Application.Current.Properties["OracleUserName"] as string + "/open")).Content.ReadAsStringAsync().Result;
            //string var = "{\"get_work_list\":[{\"notification_id\":707859,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002820  waiting for approval\",\"language\":\"US\",\"begin_date\":\"23-JUN-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"539841-482027\",\"process_name\":null,\"action\":\"N\"},{\"notification_id\":707858,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002821  has been approved\",\"language\":\"US\",\"begin_date\":\"23-JUN-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"539842-482029\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":707856,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002834 has been approved\",\"language\":\"US\",\"begin_date\":\"23-JUN-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"785036-482062\",\"process_name\":null,\"action\":\"N\"},{\"notification_id\":707854,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002835 has been approved\",\"language\":\"US\",\"begin_date\":\"23-JUN-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"785037-482063\",\"process_name\":null,\"action\":\"N\"},{\"notification_id\":707852,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002827 has been approved\",\"language\":\"US\",\"begin_date\":\"23-JUN-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"784886-482061\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":707850,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002837 has been approved\",\"language\":\"US\",\"begin_date\":\"23-JUN-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"785098-482051\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":707114,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002836 has been approved\",\"language\":\"US\",\"begin_date\":\"18-JUN-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"785069-481712\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":706816,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002832 has been approved\",\"language\":\"US\",\"begin_date\":\"17-JUN-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"785030-481453\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":706809,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002818  has been approved\",\"language\":\"US\",\"begin_date\":\"16-JUN-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"539805-481471\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":706808,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002819  has been approved\",\"language\":\"US\",\"begin_date\":\"16-JUN-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"539806-481473\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":706590,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002833 has been approved\",\"language\":\"US\",\"begin_date\":\"16-JUN-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"785031-481410\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":706588,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002831 has been approved\",\"language\":\"US\",\"begin_date\":\"16-JUN-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"785028-481381\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":706259,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002816  has been approved\",\"language\":\"US\",\"begin_date\":\"14-JUN-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"539760-480823\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":706258,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002817  has been approved\",\"language\":\"US\",\"begin_date\":\"14-JUN-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"539763-480893\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":705568,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002815  has been approved\",\"language\":\"US\",\"begin_date\":\"10-JUN-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"539757-480774\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":705425,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002830 has been approved\",\"language\":\"US\",\"begin_date\":\"09-JUN-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"784984-480830\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":705400,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002829 has been approved\",\"language\":\"US\",\"begin_date\":\"09-JUN-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"784981-480770\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":705268,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002828 has been approved\",\"language\":\"US\",\"begin_date\":\"09-JUN-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"784961-480597\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":702482,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002814  has been approved\",\"language\":\"US\",\"begin_date\":\"22-MAY-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"538766-478906\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":702144,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002825 has been approved\",\"language\":\"US\",\"begin_date\":\"19-MAY-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"784828-478873\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":701575,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002811  has been approved\",\"language\":\"US\",\"begin_date\":\"14-MAY-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"538731-478391\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":701200,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002813  has been approved\",\"language\":\"US\",\"begin_date\":\"12-MAY-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"538735-478322\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":701199,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002812  has been approved\",\"language\":\"US\",\"begin_date\":\"12-MAY-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"538734-478320\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":701185,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002809  has been approved\",\"language\":\"US\",\"begin_date\":\"11-MAY-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"538728-478188\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":701184,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002806  has been approved\",\"language\":\"US\",\"begin_date\":\"11-MAY-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"538724-478176\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":701183,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002807  has been approved\",\"language\":\"US\",\"begin_date\":\"11-MAY-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"538726-478179\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":701182,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002808  has been approved\",\"language\":\"US\",\"begin_date\":\"11-MAY-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"538727-478181\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":701181,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002810  has been approved\",\"language\":\"US\",\"begin_date\":\"11-MAY-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"538730-478249\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":701134,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002823 has been approved\",\"language\":\"US\",\"begin_date\":\"11-MAY-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"784755-478288\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":701132,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002824 has been approved\",\"language\":\"US\",\"begin_date\":\"11-MAY-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"784760-478287\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":701029,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002821 has been approved\",\"language\":\"US\",\"begin_date\":\"11-MAY-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"784742-478073\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":701027,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002822 has been approved\",\"language\":\"US\",\"begin_date\":\"11-MAY-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"784754-478186\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":700993,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002818 has been approved\",\"language\":\"US\",\"begin_date\":\"10-MAY-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"784738-478063\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":700894,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002817 has been approved\",\"language\":\"US\",\"begin_date\":\"10-MAY-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"784737-478062\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":700892,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002820 has been approved\",\"language\":\"US\",\"begin_date\":\"10-MAY-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"784741-478069\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":700890,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002819 has been approved\",\"language\":\"US\",\"begin_date\":\"10-MAY-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"784740-478070\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":700836,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002805  has been approved\",\"language\":\"US\",\"begin_date\":\"09-MAY-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"538704-477897\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":700257,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002816 has been approved\",\"language\":\"US\",\"begin_date\":\"06-MAY-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"784071-476842\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":699214,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002804  has been approved\",\"language\":\"US\",\"begin_date\":\"03-MAY-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"537888-476569\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":699213,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002803  has been approved\",\"language\":\"US\",\"begin_date\":\"03-MAY-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"537862-476235\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":699064,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002813 has been approved\",\"language\":\"US\",\"begin_date\":\"02-MAY-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"783984-476167\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":698896,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002797  has been approved\",\"language\":\"US\",\"begin_date\":\"29-APR-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"537838-475944\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":698895,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002798  has been approved\",\"language\":\"US\",\"begin_date\":\"29-APR-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"537839-476149\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":698894,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002800  has been approved\",\"language\":\"US\",\"begin_date\":\"29-APR-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"537859-476221\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":698893,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002796  has been approved\",\"language\":\"US\",\"begin_date\":\"29-APR-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"537837-475942\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":698892,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002799  has been approved\",\"language\":\"US\",\"begin_date\":\"29-APR-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"537858-476219\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":698889,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002802  has been approved\",\"language\":\"US\",\"begin_date\":\"29-APR-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"537861-476231\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":698888,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002801  has been approved\",\"language\":\"US\",\"begin_date\":\"29-APR-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"537860-476223\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":698398,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002815 has been approved\",\"language\":\"US\",\"begin_date\":\"27-APR-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"783988-476225\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":698354,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002810 has been approved\",\"language\":\"US\",\"begin_date\":\"27-APR-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"783980-476150\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":698352,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002811 has been approved\",\"language\":\"US\",\"begin_date\":\"27-APR-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"783981-476154\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":698350,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002812 has been approved\",\"language\":\"US\",\"begin_date\":\"27-APR-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"783982-476160\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":698348,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002814 has been approved\",\"language\":\"US\",\"begin_date\":\"27-APR-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"783985-476180\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":698346,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002809 has been approved\",\"language\":\"US\",\"begin_date\":\"27-APR-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"783977-476146\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":697808,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002808 has been approved\",\"language\":\"US\",\"begin_date\":\"25-APR-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"783968-475870\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":697806,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002807 has been approved\",\"language\":\"US\",\"begin_date\":\"25-APR-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"783967-475869\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":697379,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002806 has been approved\",\"language\":\"US\",\"begin_date\":\"21-APR-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"783928-475556\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":696859,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002794  has been approved\",\"language\":\"US\",\"begin_date\":\"18-APR-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"537775-475167\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":696858,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002795  has been approved\",\"language\":\"US\",\"begin_date\":\"18-APR-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"537776-475215\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":696744,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002801 has been approved\",\"language\":\"US\",\"begin_date\":\"16-APR-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"783846-474988\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":696571,\"from_user\":\"Puthiyottil, Hapheel\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"PDF Document Generation Failure - Standard Purchase Order 45002791\",\"language\":\"US\",\"begin_date\":\"16-APR-2020\",\"type\":\"PO Approval\",\"from_role\":\"HPUTHIYOTTIL\",\"item_key\":\"537769-475032\",\"process_name\":null,\"action\":\"N\"},{\"notification_id\":696570,\"from_user\":\"Puthiyottil, Hapheel\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"PDF Document Generation Failure - Standard Purchase Order 45002792\",\"language\":\"US\",\"begin_date\":\"16-APR-2020\",\"type\":\"PO Approval\",\"from_role\":\"HPUTHIYOTTIL\",\"item_key\":\"537770-475034\",\"process_name\":null,\"action\":\"N\"},{\"notification_id\":696569,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002792  has been approved\",\"language\":\"US\",\"begin_date\":\"16-APR-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"537770-475034\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":696568,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002789  has been approved\",\"language\":\"US\",\"begin_date\":\"16-APR-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"537767-475028\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":696567,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002790  has been approved\",\"language\":\"US\",\"begin_date\":\"16-APR-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"537768-475030\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":696566,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002791  has been approved\",\"language\":\"US\",\"begin_date\":\"16-APR-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"537769-475032\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":696565,\"from_user\":\"Al Kuwari, Ali\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Standard Purchase Order  45002793  has been approved\",\"language\":\"US\",\"begin_date\":\"16-APR-2020\",\"type\":\"PO Approval\",\"from_role\":\"ALI\",\"item_key\":\"537773-475060\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":696528,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002805 has been approved\",\"language\":\"US\",\"begin_date\":\"15-APR-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"783855-475067\",\"process_name\":null,\"action\":\"R\"},{\"notification_id\":696497,\"from_user\":\"Kuni, Mustafa\",\"selected_person_id\":0,\"transaction_id\":0,\"to_user\":\"Puthiyottil, Hapheel\",\"subject\":\"Purchase Requisition 35002804 has been approved\",\"language\":\"US\",\"begin_date\":\"15-APR-2020\",\"type\":\"Requisition\",\"from_role\":\"MPULIYAMKUNI\",\"item_key\":\"783852-475047\",\"process_name\":null,\"action\":\"N\"}]}";
            DataInfo.WorkListInformation = JsonConvert.DeserializeObject<WorkListResponse>(result);
            if (result != null)
            {
                if (DataInfo.WorkListInformation.get_work_list.Count > 0)
                {
                    for (int i = 0; i < DataInfo.WorkListInformation.get_work_list.Count; i++)
                    {
                        switch (DataInfo.WorkListInformation.get_work_list[i].action)
                        {
                            case "N":
                                DataInfo.WorkListInformation.get_work_list[i].image_Name = "ic_approval.png";   
                                break;                            
                            default:
                                switch (DataInfo.WorkListInformation.get_work_list[i].process_name)
                                {
                                    case "XXHF_EMP_LEAVE_AMDT":
                                        DataInfo.WorkListInformation.get_work_list[i].image_Name = "ic_amend_notif.png";
                                        break;
                                    case "XXHF_LEAVE_CANCEL":
                                        DataInfo.WorkListInformation.get_work_list[i].image_Name = "ic_cancel_notif.png";
                                        break;
                                    case "XXHF_EMP_DUTY_VISIT_REQ":
                                        DataInfo.WorkListInformation.get_work_list[i].image_Name = "ic_duty_notif.png";
                                        break;
                                    case "HR_GENERIC_APPROVAL_PRC":
                                        DataInfo.WorkListInformation.get_work_list[i].image_Name = "ic_leave_notif.png";
                                        break;
                                    case "XXHF_EMP_EXIT_PERMIT_REQ":
                                        DataInfo.WorkListInformation.get_work_list[i].image_Name = "ic_permit_notif.png";
                                        break;
                                    case "XXHF_EMP_SSHR_LR":
                                        DataInfo.WorkListInformation.get_work_list[i].image_Name = "ic_resume_notif.png";
                                        break;

                                    case "XXHF_EMP_REIMB_REQ":
                                        DataInfo.WorkListInformation.get_work_list[i].image_Name = "ic_clubmembership_not.png";
                                        break;
                                    case "XXHF_EMP_EDUC_REQ":
                                        DataInfo.WorkListInformation.get_work_list[i].image_Name = "ic_educationallowance_not.png";
                                        break;
                                    case "XXHF_LETTER_REQUEST_EMP":
                                        DataInfo.WorkListInformation.get_work_list[i].image_Name = "ic_empletterrequest_not.png";
                                        break;
                                    case "XXHF_EMP_NEW_HIRE_SAL_REQ":
                                        DataInfo.WorkListInformation.get_work_list[i].image_Name = "ic_salaryadvance_not.png";
                                        break;
                                    case "XXHF_EMP_APP_ACCESS":
                                        DataInfo.WorkListInformation.get_work_list[i].image_Name = "ic_appliactionaccessrequest_not.png";
                                        break;
                                    case "XXHF_EMP_CAR_LOAN_REQ":
                                        DataInfo.WorkListInformation.get_work_list[i].image_Name = "ic_carloan_not.png";
                                        break;
                                    default:
                                        DataInfo.WorkListInformation.get_work_list[i].image_Name = "ic_notification.png";
                                        break;
                                }
                                break;
                        }
                    }
                }
            }
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> GetNotificationDetailHistory(string itemkey, int selected_person_id)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_notification_details/" + selected_person_id + "/" + itemkey));
            DataInfo.NotificationDetailInformation = JsonConvert.DeserializeObject<NotificationDetailResponse>(result.Content.ReadAsStringAsync().Result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> GetLeaveSummary()
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            LeaveSummaryRequest request = new LeaveSummaryRequest()
            {
                p_user_name = Application.Current.Properties["OracleUserName"] as string,
                p_person_id = DataInfo.UserInformation.hf_user_auth[0].person_id,
                p_start_date = "",
                p_end_date = "",
                p_approval = "",
                p_absence_status = "",
                p_absence_type = "",
                p_absence_category = ""
            };
            var temp = JsonConvert.SerializeObject(request);
            var content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");
            var result = (await client.PostAsync(Constants.Oracle_URL + "wshr/get_leave_summary", content)).Content.ReadAsStringAsync().Result;
            DataInfo.LeaveSummaryInformation = JsonConvert.DeserializeObject<LeaveSummaryResponse>(result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> GetAbsenceType()
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_absence_types_p")).Content.ReadAsStringAsync().Result;
            DataInfo.GetAbsenceTypeInformation = JsonConvert.DeserializeObject<GetAbsenceResponse>(result);
            if (DataInfo.GetAbsenceTypeInformation != null && DataInfo.GetAbsenceTypeInformation.get_absence_types_p.Count > 0)
            {
                DataInfo.GetAbsenceTypeInformation.NewAbsenceType = new System.Collections.Generic.List<string>();
                foreach (var item in DataInfo.GetAbsenceTypeInformation.get_absence_types_p)
                    DataInfo.GetAbsenceTypeInformation.NewAbsenceType.Add(item.name);
            }
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> CalculateAbsenceDuration(string leaveType, DateTime StartDate, DateTime EndDate)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/calculate_leave_duration/" + leaveType + "/" + StartDate.ToString(format: "dd-MMM-yyyy") + "/" + EndDate.ToString(format: "dd-MMM-yyyy"))).Content.ReadAsStringAsync().Result;
            DataInfo.CalculateAbsenceDurationInformation = JsonConvert.DeserializeObject<CalculateAbsenceDurationResponse>(result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> PutLeaveRequest(LeaveRequest leave)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();            
            client.Timeout = TimeSpan.FromSeconds(180);
            var temp = JsonConvert.SerializeObject(leave);
            var content = new StringContent(JsonConvert.SerializeObject(leave), System.Text.Encoding.UTF8, "application/json");
            var result = (await client.PostAsync(Constants.Oracle_URL + "wshr/create_sshr_absence", content)).Content.ReadAsStringAsync().Result;
            DataInfo.RequestInformation = JsonConvert.DeserializeObject<RequestResponse>(result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> PutDutyVisitRequest(DutyVisitRequest request)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var temp = JsonConvert.SerializeObject(request);
            var content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");
            var result = (await client.PostAsync(Constants.Oracle_URL + "wshr/duty_visit", content)).Content.ReadAsStringAsync().Result;
            DataInfo.RequestInformation = JsonConvert.DeserializeObject<RequestResponse>(result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> PutExitPermitRequest(ExitPermitRequest request)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var temp = JsonConvert.SerializeObject(request);
            var content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");
            var result = (await client.PostAsync(Constants.Oracle_URL + "wshr/exit_permit", content)).Content.ReadAsStringAsync().Result;
            DataInfo.RequestInformation = JsonConvert.DeserializeObject<RequestResponse>(result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> GetDutyVisitPerDiemClass()
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_per_diam_class/" + DataInfo.UserInformation.hf_user_auth[0].person_id)).Content.ReadAsStringAsync().Result;
            DataInfo.DutyVisitPerDiemClassInformation = JsonConvert.DeserializeObject<DutyVisitPerDiemClassResponse>(result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> PutAttachmentsRequest(byte[] file, string filename, string transid)
        {
            var content = new ByteArrayContent(file);
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("p_user_name", Application.Current.Properties["OracleUserName"] as string);
            client.DefaultRequestHeaders.Add("p_transaction_id", transid);
            client.DefaultRequestHeaders.Add("p_file_name", filename);
            client.Timeout = TimeSpan.FromSeconds(90);
            var result = (await client.PostAsync(Constants.Oracle_URL + "wshr/add_attachment", content));
            DataInfo.AttachmentsRequestInformation = JsonConvert.DeserializeObject<RequestResponse>(result.Content.ReadAsStringAsync().Result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> GetAttachmentsInformation(string transaction_id)
            {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_attachment/" + transaction_id));
            DataInfo.GetAttachmentInformation = JsonConvert.DeserializeObject<GetAttachmentResponse>(result.Content.ReadAsStringAsync().Result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<byte[]> DownloadAttachmentsFile(string transaction_id, string file_id)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_attachment_lob/" + transaction_id + "/" + file_id));
            // Convert the string back to a byte array.
            return result.Content.ReadAsByteArrayAsync().Result;
        }

        public async Task<bool> GetLeaveBalance()
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_entitlement/" + DataInfo.UserInformation.hf_user_auth[0].person_id.ToString()));
            DataInfo.GetLeaveBalanceInformation = JsonConvert.DeserializeObject<GetLeaveBalanceResponse>(result.Content.ReadAsStringAsync().Result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> GetLeaveAppliedDates()
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_leave_applied_dates/" + DataInfo.UserInformation.hf_user_auth[0].person_id)).Content.ReadAsStringAsync().Result;
            DataInfo.GetLeaveAppliedDatesInformation = JsonConvert.DeserializeObject<LeaveAppliedDatesResponse>(result);
            if (DataInfo.GetLeaveAppliedDatesInformation != null && DataInfo.GetLeaveAppliedDatesInformation.items.Count > 0)
            {
                DataInfo.GetLeaveAppliedDatesInformation.NewAppliedLeave = new System.Collections.Generic.List<string>();
                foreach (var item in DataInfo.GetLeaveAppliedDatesInformation.items)
                    DataInfo.GetLeaveAppliedDatesInformation.NewAppliedLeave.Add(item.date_start_end);
            }
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> PutLeaveAmendmentRequest(LeaveAmendmentRequest request)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var temp = JsonConvert.SerializeObject(request);
            var content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");
            var result = (await client.PostAsync(Constants.Oracle_URL + "wshr/leave_amend", content)).Content.ReadAsStringAsync().Result;
            DataInfo.RequestInformation = JsonConvert.DeserializeObject<RequestResponse>(result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> PutLeaveCancelRequest(LeaveCancelRequest request)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var temp = JsonConvert.SerializeObject(request);
            var content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");
            var result = (await client.PostAsync(Constants.Oracle_URL + "wshr/leave_cancel", content)).Content.ReadAsStringAsync().Result;
            DataInfo.RequestInformation = JsonConvert.DeserializeObject<RequestResponse>(result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> PutLeaveResumeRequest(LeaveResumeRequest request)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var temp = JsonConvert.SerializeObject(request);
            var content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");
            var result = (await client.PostAsync(Constants.Oracle_URL + "wshr/leave_resume", content)).Content.ReadAsStringAsync().Result;
            DataInfo.RequestInformation = JsonConvert.DeserializeObject<RequestResponse>(result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> PutActionRequest(ActionRequest request)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var temp = JsonConvert.SerializeObject(request);
            var content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");
            var result = (await client.PostAsync(Constants.Oracle_URL + "wshr/action", content));
            DataInfo.RequestInformation = JsonConvert.DeserializeObject<RequestResponse>(result.Content.ReadAsStringAsync().Result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> GetActionHistory(string NotificationID)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_action_hist/" + Application.Current.Properties["OracleUserName"] as string + "/" + NotificationID));
            DataInfo.ActionHistoryInformation = JsonConvert.DeserializeObject<ActionHistoryResponse>(result.Content.ReadAsStringAsync().Result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> GetReassignUser()
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_reassign_users/asd"));
            DataInfo.ReassignUserInformation = JsonConvert.DeserializeObject<ReassignUserResponse>(result.Content.ReadAsStringAsync().Result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> GetLeaveSitSummary()
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_leave_sit_summary/" + Application.Current.Properties["OracleUserName"] as string + "/" + DataInfo.UserInformation.hf_user_auth[0].person_id));
            DataInfo.GetLeaveSitSummaryInformation = JsonConvert.DeserializeObject<GetLeaveSitSummaryResponse>(result.Content.ReadAsStringAsync().Result);
            if (DataInfo.GetLeaveSitSummaryInformation.get_leave_sit_summary.Count > 0)
            {               
                for (int i = 0; i < DataInfo.GetLeaveSitSummaryInformation.get_leave_sit_summary.Count; i++)
                {
                    if (DataInfo.GetLeaveSitSummaryInformation.get_leave_sit_summary[i].start_date != null && DataInfo.GetLeaveSitSummaryInformation.get_leave_sit_summary[i].end_date != null)
                        DataInfo.GetLeaveSitSummaryInformation.get_leave_sit_summary[i].leave_type += " (" + DataInfo.GetLeaveSitSummaryInformation.get_leave_sit_summary[i].start_date + " to " + DataInfo.GetLeaveSitSummaryInformation.get_leave_sit_summary[i].end_date + ")";
                }
            }
            if (result != null)
                return true;
            return false;
        }

        //Phase 2

        public async Task<bool> GetCarLoanDropDown()
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_carloan_lov/" + DataInfo.UserInformation.hf_user_auth[0].person_id));
            DataInfo.CarLoanDropDownInformation = JsonConvert.DeserializeObject<CarLoanDropDownResponse>(result.Content.ReadAsStringAsync().Result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> GetEducationAllowanceDropDown()
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_educationallowance_lov/" + DataInfo.UserInformation.hf_user_auth[0].person_id));
            DataInfo.EducationAllowanceDropDownInformation = JsonConvert.DeserializeObject<EducationAllowanceDropDownResponse>(result.Content.ReadAsStringAsync().Result);
            DataInfo.EducationAllowanceDropDownInformation.p_accademic_year_list = DataInfo.EducationAllowanceDropDownInformation.p_accademic_year_list.Replace("\\","");
            DataInfo.EducationAllowanceDropDownInformation.p_payable_to = DataInfo.EducationAllowanceDropDownInformation.p_payable_to.Replace("\\", "");
            DataInfo.EducationAllowanceAccademicYearInformation = JsonConvert.DeserializeObject<EducationAllowanceAccademicYear>(DataInfo.EducationAllowanceDropDownInformation.p_accademic_year_list);
            DataInfo.EducationAllowancePayableToInformation = JsonConvert.DeserializeObject<EducationAllowancePayableTo>(DataInfo.EducationAllowanceDropDownInformation.p_payable_to);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> GetEducationAllowanceDDOB(string personid)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_educationallowance_ddob/" + personid));
            DataInfo.EducationAllowanceDDOBInformation = JsonConvert.DeserializeObject<EducationAllowanceDDOBResponse>(result.Content.ReadAsStringAsync().Result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> GetApplicationAccessDropDown()
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_applicationaccess_lov/" + DataInfo.UserInformation.hf_user_auth[0].person_id));
            DataInfo.ApplicationAccessDropDownInformation = JsonConvert.DeserializeObject<ApplicationAccessDropDownResponse>(result.Content.ReadAsStringAsync().Result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> GetApplicationAccessRName(string applicationid)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_applicationaccess_rname/" + applicationid));
            DataInfo.ApplicationAccessRNameInformation = JsonConvert.DeserializeObject<ApplicationAccessRNameResponse>(result.Content.ReadAsStringAsync().Result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> GetEmployeeLetterDropDown()
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_employeeletter_lov"));
            DataInfo.EmployeeLetterDropDownInformation = JsonConvert.DeserializeObject<EmployeeLetterDropDownResponse>(result.Content.ReadAsStringAsync().Result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> GetReimbursementDropDown()
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_reimbursement_lov"));
            DataInfo.ReimbursementDropDownInformation = JsonConvert.DeserializeObject<ReimbursementDropDownResponse>(result.Content.ReadAsStringAsync().Result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> GetReimbursementAmount(string reimbursement_type)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_reimbursement_amt/" + DataInfo.UserInformation.hf_user_auth[0].person_id + "/" + reimbursement_type));
            DataInfo.ReimbursementAmountInformation = JsonConvert.DeserializeObject<ReimbursementAmountResponse>(result.Content.ReadAsStringAsync().Result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> PutEducationAllowanceRequest(EducationAllowanceServiceRequest request)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var temp = JsonConvert.SerializeObject(request);
            var content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");
            var result = (await client.PostAsync(Constants.Oracle_URL + "wshr/education_allowance", content)).Content.ReadAsStringAsync().Result;
            DataInfo.RequestInformation = JsonConvert.DeserializeObject<RequestResponse>(result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> PutCarLoanRequest(CarLoanServiceRquest request)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var temp = JsonConvert.SerializeObject(request);
            var content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");
            var result = (await client.PostAsync(Constants.Oracle_URL + "wshr/car_loan", content)).Content.ReadAsStringAsync().Result;
            DataInfo.RequestInformation = JsonConvert.DeserializeObject<RequestResponse>(result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> PutSalaryAdvanceRequest(SalaryAdvanceServiceRequest request)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var temp = JsonConvert.SerializeObject(request);
            var content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");
            var result = (await client.PostAsync(Constants.Oracle_URL + "wshr/salary_advance", content)).Content.ReadAsStringAsync().Result;
            DataInfo.RequestInformation = JsonConvert.DeserializeObject<RequestResponse>(result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> PutApplicationAccessRequest(ApplicationAccessServiceRequest request)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var temp = JsonConvert.SerializeObject(request);
            var content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");
            var result = (await client.PostAsync(Constants.Oracle_URL + "wshr/application_access", content)).Content.ReadAsStringAsync().Result;
            DataInfo.RequestInformation = JsonConvert.DeserializeObject<RequestResponse>(result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> PutEmployeeLetterRequest(EmployeeLetterServiceRequest request)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var temp = JsonConvert.SerializeObject(request);
            var content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");
            var result = (await client.PostAsync(Constants.Oracle_URL + "wshr/letter_request", content)).Content.ReadAsStringAsync().Result;
            DataInfo.RequestInformation = JsonConvert.DeserializeObject<RequestResponse>(result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> PutReimbursementRequest(ReimbursementServiceRequest request)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var temp = JsonConvert.SerializeObject(request);
            var content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");
            var result = (await client.PostAsync(Constants.Oracle_URL + "wshr/reimbursement_request", content)).Content.ReadAsStringAsync().Result;
            DataInfo.RequestInformation = JsonConvert.DeserializeObject<RequestResponse>(result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> PutAirTicketRequest(AirTicketServiceRequest request)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var temp = JsonConvert.SerializeObject(request);
            var content = new StringContent(JsonConvert.SerializeObject(request), System.Text.Encoding.UTF8, "application/json");
            var result = (await client.PostAsync(Constants.Oracle_URL + "wshr/air_ticket", content)).Content.ReadAsStringAsync().Result;
            DataInfo.RequestInformation = JsonConvert.DeserializeObject<RequestResponse>(result);
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> GetApproverUserList()
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_approver_user_list/" + Application.Current.Properties["OracleUserName"] as string));
            DataInfo.GetApproverUserListInformation = JsonConvert.DeserializeObject<GetApproverUserListResponse>(result.Content.ReadAsStringAsync().Result);
            if (DataInfo.GetApproverUserListInformation.get_approver_user_list.Count > 0)
            {
                for (int i = 0; i < DataInfo.GetApproverUserListInformation.get_approver_user_list.Count; i++)
                {
                    await GetFutureLeaveSitSummary(DataInfo.GetApproverUserListInformation.get_approver_user_list[i].user_name, DataInfo.GetApproverUserListInformation.get_approver_user_list[i].person_id);
                }
            }
            if (DataInfo.GetFutureLeaveSitSummaryInformation.get_future_leave_sit_summary.Count > 0)
            {
                for (int i = 0; i < DataInfo.GetFutureLeaveSitSummaryInformation.get_future_leave_sit_summary.Count; i++)
                {
                    if (DataInfo.GetFutureLeaveSitSummaryInformation.get_future_leave_sit_summary[i].start_date != null && DataInfo.GetFutureLeaveSitSummaryInformation.get_future_leave_sit_summary[i].end_date != null)
                        DataInfo.GetFutureLeaveSitSummaryInformation.get_future_leave_sit_summary[i].leave_type += " (" + DataInfo.GetFutureLeaveSitSummaryInformation.get_future_leave_sit_summary[i].start_date + " to " + DataInfo.GetFutureLeaveSitSummaryInformation.get_future_leave_sit_summary[i].end_date + ")";
                }
            }
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> GetFutureLeaveSitSummary(string username, int personid)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_future_leave_sit_summary/" + username + "/" + personid));
            if (DataInfo.GetFutureLeaveSitSummaryInformation != null && DataInfo.GetFutureLeaveSitSummaryInformation.get_future_leave_sit_summary.Count > 0)
            {
                var temp = JsonConvert.DeserializeObject<GetFutureLeaveSitSummaryResponse>(result.Content.ReadAsStringAsync().Result);
                if (temp.get_future_leave_sit_summary.Count > 0)
                {
                    foreach (var item in temp.get_future_leave_sit_summary)
                        DataInfo.GetFutureLeaveSitSummaryInformation.get_future_leave_sit_summary.Add(item);
                }

            }
            else
            {
                DataInfo.GetFutureLeaveSitSummaryInformation = JsonConvert.DeserializeObject<GetFutureLeaveSitSummaryResponse>(result.Content.ReadAsStringAsync().Result);
            }
            if (result != null)
                return true;
            return false;
        }

        public async Task<bool> GetApproverUserListLeaveBalance()
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_approver_user_list/" + Application.Current.Properties["OracleUserName"] as string));
            DataInfo.GetApproverUserListLeaveBalanceInformation = JsonConvert.DeserializeObject<GetApproverUserListResponse>(result.Content.ReadAsStringAsync().Result);
            if (DataInfo.GetApproverUserListLeaveBalanceInformation.get_approver_user_list.Count > 0)
            {
                for (int i = 0; i < DataInfo.GetApproverUserListLeaveBalanceInformation.get_approver_user_list.Count; i++)
                {
                    var resultitem = await GetLeaveBalance(DataInfo.GetApproverUserListLeaveBalanceInformation.get_approver_user_list[i].person_id);
                    if (resultitem != null)
                        DataInfo.GetApproverUserListLeaveBalanceInformation.get_approver_user_list[i].leave_balance = resultitem;
                }
            }            
            if (result != null)
                return true;
            return false;
        }

        public async Task<string> GetLeaveBalance(int person_id)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_entitlement/" + person_id));
            var resultitem = JsonConvert.DeserializeObject<GetLeaveBalanceResponse>(result.Content.ReadAsStringAsync().Result);
            if (result != null)
                return resultitem.p_entitlement;
            return null;
        }


        public async Task<bool> GetPONotificationDetails(string item_key)
        { 
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);
           
          

            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_po_notification_details/" + item_key));
            DataInfo.PONotificationDetailResponseInformation = JsonConvert.DeserializeObject<PONotificationDetailResponse>(result.Content.ReadAsStringAsync().Result);
            if (DataInfo.PONotificationDetailResponseInformation != null)
                return true;
            return false;
        }


        public async Task<bool> GetRequisitionNotificationDetails(string item_key)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);


  
    


            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_req_notification_details/" + item_key));
            DataInfo.RequisitionNotificationDetailResponseInformation = JsonConvert.DeserializeObject<RequisitionNotificationDetailResponse>(result.Content.ReadAsStringAsync().Result);
            if (DataInfo.RequisitionNotificationDetailResponseInformation != null)
                return true;
            return false;
        }



        public async Task<bool> GetTransactionWorkFlow(string item_key)
        {
            DependencyService.Get<SavePDF>().TrustCA();
            HttpClient client = new HttpClient();
            client.Timeout = TimeSpan.FromSeconds(30);



         
   // http://tstmapp101.hassad.local:8081/ords/apps/wshr/get_payment_notification_details/24320



            var result = (await client.GetAsync(Constants.Oracle_URL + "wshr/get_payment_notification_details/" + item_key));
            DataInfo.HassadTransactionWorkFLowResponseInformation = JsonConvert.DeserializeObject<HassadTransactionWorkFLow>(result.Content.ReadAsStringAsync().Result);
            if (DataInfo.HassadTransactionWorkFLowResponseInformation != null)
                return true;
            return false;
        }


    }
}
