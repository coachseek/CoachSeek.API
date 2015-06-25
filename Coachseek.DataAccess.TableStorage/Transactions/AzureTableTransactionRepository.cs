//using CoachSeek.Common;
//using CoachSeek.Data.Model;
//using Coachseek.DataAccess.TableStorage.Emailing;
//using CoachSeek.Domain.Entities;
//using CoachSeek.Domain.Repositories;
//using Microsoft.WindowsAzure.Storage.Table;

//namespace Coachseek.DataAccess.TableStorage.Transactions
//{
//    public class AzureTableTransactionRepository : AzureTableRepositoryBase, ITransactionRepository
//    {
//        protected override string TableName { get { return "transactions"; } }



//        public void Save(string emailAddress)
//        {
//            if (Get(emailAddress)) 
//                return;

//            var transaction = new TransactionEntity(emailAddress) { EmailAddress = emailAddress };
//            Table.Execute(TableOperation.Insert(transaction));
//        }


//        public bool Get(string emailAddress)
//        {
//            var parts = emailAddress.Split('@');
//            var retrieveOperation = TableOperation.Retrieve<EmailAddressEntity>(parts[1], parts[0]);

//            var retrievedResult = Table.Execute(retrieveOperation);
//            return retrievedResult.Result != null;
//        }

//        public TransactionData GetTransaction(string id)
//        {
//            throw new System.NotImplementedException();
//        }

//        public TransactionData GetTransaction(string id, TransactionType type)
//        {
//            throw new System.NotImplementedException();
//        }

//        public TransactionData AddTransaction(Transaction transaction)
//        {
//            throw new System.NotImplementedException();
//        }

//        public Payment GetPayment(string id)
//        {
//            throw new System.NotImplementedException();
//        }

//        public void AddPayment(NewPayment payment)
//        {
//            throw new System.NotImplementedException();
//        }

//        public void VerifyPayment(Payment payment)
//        {
//            throw new System.NotImplementedException();
//        }
//    }
//}
