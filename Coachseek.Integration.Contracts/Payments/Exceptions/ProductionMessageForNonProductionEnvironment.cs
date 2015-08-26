namespace Coachseek.Integration.Contracts.Payments.Exceptions
{
    public class ProductionMessageForNonProductionEnvironment : PaymentProcessingException
    {
        public override string Message
        {
            get { return "Production message received for non-production environment."; }
        }
    }
}
