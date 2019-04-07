using System;

namespace testsripe
{

    using System.Linq;
    using ServiceStack;
    using ServiceStack.Text;
    using System;
    using ServiceStack.Caching;
    using ServiceStack.Redis;
    using System.Collections.Generic;

    public class SubscriptionAccount
    {


        public long acct_id { get; set; }
        public string user_id { get; set; }
        public string email_address { get; set; }
        public string subscription_payment_id { get; set; }
        public string api_key { get; set; }
        public DateTime created_date { get; set; }
        public DateTime start_current_month { get; set; }
        public Stripe.Subscription stripe_subscription { get; set; }
        public Stripe.Customer stripe_customer { get; set; }
    }



    class Program
    {
        static void Main(string[] args)
        {
            var redisManager = new RedisManagerPool("localhost:6379");


            using (var redis = redisManager.GetCacheClient())
            {
                try
                {
                    var json = "{\"acct_id\":1016,\"user_id\":\"17\",\"email_address\":\"kevin10@xxxxx.net\",\"api_key\":\"asfasfasfasd\",\"subscription\":{\"subscription_id\":1,\"urls_per_month\":1000,\"max_retention_days\":15,\"cost_per_month_usd\":5.00},\"created_date\":\"2019-04-06T19:19:59.4200190Z\",\"start_current_month\":\"2019-04-06T19:19:59.4200900Z\",\"stripe_subscription\":{\"id\":\"safasdsda\",\"object\":\"subscription\",\"billing\":\"charge_automatically\",\"billingCycleAnchor\":\"2019-04-06T19:19:55.0000000Z\",\"cancelAtPeriodEnd\":false,\"created\":\"2019-04-06T19:19:55.0000000Z\",\"currentPeriodEnd\":\"2019-04-13T19:19:55.0000000Z\",\"currentPeriodStart\":\"2019-04-06T19:19:55.0000000Z\",\"items\":[{\"id\":\"sadf\",\"object\":\"subscription_item\",\"created\":\"2019-04-06T19:19:56.0000000Z\",\"metadata\":{},\"plan\":{\"id\":\"plan_EpYQHnD2Ksn25r\",\"object\":\"plan\",\"active\":true,\"aggregateUsage\":\"sum\",\"amount\":1,\"billingScheme\":\"per_unit\",\"created\":\"2019-04-05T14:46:52.0000000Z\",\"currency\":\"usd\",\"interval\":\"week\",\"intervalCount\":1,\"livemode\":false,\"metadata\":{},\"nickname\":\"per url\",\"usageType\":\"metered\"},\"quantity\":0,\"subscription\":\"sub_Eq03qDgdXjV8zu\"}],\"livemode\":false,\"metadata\":{},\"plan\":{\"id\":\"asdfsad\",\"object\":\"plan\",\"active\":true,\"aggregateUsage\":\"sum\",\"amount\":1,\"billingScheme\":\"per_unit\",\"created\":\"2019-04-05T14:46:52.0000000Z\",\"currency\":\"usd\",\"interval\":\"week\",\"intervalCount\":1,\"livemode\":false,\"metadata\":{},\"nickname\":\"per url\",\"usageType\":\"metered\"},\"quantity\":1,\"start\":\"2019-04-06T19:19:55.0000000Z\",\"status\":\"active\"},\"stripe_customer\":{\"id\":\"cus_Eq03mgKkLr5YHj\",\"object\":\"customer\",\"accountBalance\":0,\"created\":\"2019-04-06T19:19:54.0000000Z\",\"defaultSourceId\":\"card_1EMK3SKf8n8rpwYnBMEp5u4V\",\"delinquent\":false,\"email\":\"kevin10@connells.net\",\"invoicePrefix\":\"7927E6A6\",\"invoiceSettings\":{},\"livemode\":false,\"metadata\":{},\"sources\":[{\"__type\":\"Stripe.Card, Stripe.net\",\"id\":\"card_1EMK3SKf8n8rpwYnBMEp5u4V\",\"object\":\"card\",\"brand\":\"Visa\",\"country\":\"US\",\"defaultForCurrency\":false,\"expMonth\":4,\"expYear\":2020,\"fingerprint\":\"eSS4xyRaH5gdg55C\",\"funding\":\"credit\",\"last4\":\"4242\",\"metadata\":{}}],\"subscriptions\":[]}}";
                    var acct = ServiceStack.Text.JsonSerializer.DeserializeFromString<SubscriptionAccount>(json);

                    //items are null, try to put it back in and reserialize
                    acct.stripe_subscription.Items = new Stripe.StripeList<Stripe.SubscriptionItem>();
                    acct.stripe_subscription.Items.Data = new List<Stripe.SubscriptionItem>();

                    var json2 = acct.ToJson();
                    var acct2 = ServiceStack.Text.JsonSerializer.DeserializeFromString<SubscriptionAccount>(json2);


                    if (acct2.stripe_subscription.Items == null)
                        throw new Exception("Items null");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

            }

        }
    }
}
