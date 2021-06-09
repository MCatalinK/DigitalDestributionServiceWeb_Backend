namespace DigitalDistribution.Models.Constants
{
    public static class StringConstants
    {
        //Item already exists exceptions
        public static string AlreadyExists = "Item already exists";
        public static string AddressAlreadyExists = "The current user has already an address";
        public static string DevTeamExists = "There is already a dev team with the same name";
        public static string InvoiceExists = "There is already an invoice that hasn't been payed";
        public static string InvoiceProductExists = "This product already exists on the current invoice";
        public static string LibraryItemExists = "This item is already in the user's library";
        public static string ReviewExists = "The user made already a review for this product";
        public static string ProfileExists = "The user has already a profile";

        //Not found exceptions
        public static string BillingAddressNotFound = " Billing address wasn't found";
        public static string NoDevTeams = "There are no dev teams";
        public static string NoDevTeamFound = "The DevTeam wasn't found";
        public static string NoInvoicesFound = "No invoices found";
        public static string NoProductFound = "No products found";
        public static string UpdateNotFound = "The update wasn't found";
        public static string ProfileNotFound = "No profile found";
        public static string NoReviewFound = "No Review Found";
        public static string LibraryNotFound = "Library not found";

        //Bad request exceptions
        public static string BadReviewRatingEx = "The Rating can't be higher than 10 or lower than 0";
        public static string BadProductPriceEx = "The price can't be lower than 0";

    }
}
