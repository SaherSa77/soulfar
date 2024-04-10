using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SoulFar.Models;
using PayPal.Api;
using Stripe.Checkout;
using Stripe;
using PayPal;

namespace SoulFar.Controllers
{
    public class PaymentController : Controller
    {
        DataContext db = new DataContext();
        // GET: ThankYou
        public ActionResult Index()
        {
            ViewBag.cartBox = null;
            ViewBag.Total = null;
            ViewBag.NoOfItem = null;
            TempShpData.items = null;
            return View();
        }
        public ActionResult CreateCheckoutSession(PaymentVM payObj)
        {
            var products = db.OrderDetails.Where(x => x.OrderID == payObj.OrderID).ToList();
            var LineItems = new List<SessionLineItemOptions>();
            foreach (var item in products)
            {
                LineItems.Add(new SessionLineItemOptions()
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = Convert.ToInt32(item.UnitPrice) * 100,
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Name,
                        },

                    },
                    Quantity = item.Quantity
                });
            }
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                LineItems = LineItems,
                Mode = "payment",
                SuccessUrl = "https://localhost:44313/PaymentResponse/ThankYou",
                CancelUrl = "https://localhost:44313/PaymentResponse/cancel",
            };

            var service = new Stripe.Checkout.SessionService();
            Stripe.Checkout.Session session = service.Create(options);

            Response.Headers.Add("Location", session.Url);
            return new HttpStatusCodeResult(303);
        }

        public ActionResult PaymentWithPaypal(PaymentVM payment, string Cancel = null, string guid = null)
        {
            //getting the apiContext  
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                //A resource representing a Payer that funds a payment Payment Method as paypal  
                //Payer Id will be returned when payment proceeds or click to pay  
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    //this section will be executed first because PayerID doesn't exist  
                    //it is returned by the create function call of the payment class  
                    // Creating a payment  
                    // baseURL is the url on which paypal sendsback the data.  
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Payment/PaymentWithPayPal?";
                    //here we are generating guid for storing the paymentID received in session  
                    //which will be used in the payment execution  
                    guid = string.IsNullOrEmpty(guid) ? Convert.ToString((new Random()).Next(100000)) : guid;
                    //CreatePayment function gives us the payment approval url  
                    //on which payer is redirected for paypal account payment  
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid, payment);
                    //get links returned from paypal in response to Create function call  
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    // saving the paymentID in the key guid  
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This function exectues after receving all parameters for the payment  
                    guid = string.IsNullOrEmpty(guid) ? Request.Params["guid"] : guid;
                    var executedPayment = ExecutePayment(apiContext, payment);
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                }
            }
            catch (PayPalException ex)
            {
                return RedirectToAction("Cancel", "PaymentResponse");
            }
            //on successful payment, show success page to user.  
            return RedirectToAction("ThankYou", "PaymentResponse");
        }
        private PayPal.Api.Payment payment;
        private PayPal.Api.Payment ExecutePayment(APIContext apiContext, PaymentVM payObj)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = Convert.ToString(payObj.CustomerID)
            };
            this.payment = new PayPal.Api.Payment()
            {
                id = Convert.ToString(payObj.paymentID)
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private PayPal.Api.Payment CreatePayment(APIContext apiContext, string redirectUrl, PaymentVM payObj)
        {
            //create itemlist and add item objects to it  
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            var products = db.OrderDetails.Where(x => x.OrderID == payObj.OrderID).ToList();
            //Adding Item Details like name, currency, price etc  
            foreach (var item in products)
            {
                itemList.items.Add(new Item()
                {
                    name = item.Product.Name,
                    currency = "USD",
                    price = Convert.ToString(item.Product.UnitPrice),
                    quantity = Convert.ToString(item.Quantity),
                    sku = "sku" + item.Product.ProductID
                });
            }
            var user = db.Customers.FirstOrDefault(x => x.CustomerID == payObj.CustomerID);
            var payer = new Payer()
            {
                payment_method = "paypal",
                payer_info = new PayerInfo()
                {
                    payer_id = Convert.ToString(payObj.CustomerID),
                    email = user.Email,
                    first_name = user.First_Name,
                    last_name = user.Last_Name
                }
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            // Adding Tax, shipping and Subtotal details  
            var details = new Details()
            {
                tax = "0",
                shipping = "0",
                subtotal = Convert.ToString(Convert.ToInt32(payObj.TotalAmount))
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = Convert.ToString(Convert.ToInt32(payObj.TotalAmount)), // Total must be equal to sum of tax, shipping and subtotal.  
                details = details
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = "Inv#" + payObj.OrderID, //Generate an Invoice No  
                amount = amount,
                item_list = itemList
            });
            this.payment = new PayPal.Api.Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
                //create_time=DateTime.Now.ToShortDateString()
            };
            // Create a payment using a APIContext  
            return this.payment.Create(apiContext);
        }
    }
}