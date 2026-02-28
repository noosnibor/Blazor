namespace PortalWeb.Services;

public static class MemoryStoredData
{
    public static IEnumerable<PaymentType> GetPaymentType()
    {
        return
            [
                new () {flngPaymentKey = 1, fstrPaymentType = "Cash", fblnActive = true},
                new () {flngPaymentKey = 2, fstrPaymentType = "Cheque", fblnActive = true},
                new () {flngPaymentKey = 3, fstrPaymentType = "POS", fblnActive = true},
                new () {flngPaymentKey = 4, fstrPaymentType = "Wire Transfer", fblnActive = true},
                new () {flngPaymentKey = 5, fstrPaymentType = "E-Transfer", fblnActive = true},
                new () {flngPaymentKey = 6, fstrPaymentType = "Cash App", fblnActive = true},
            ];
    }



    public static IEnumerable<MemberStatus> GetMemberStatus()
    {
        return
            [
                new () {flngMemberStatusKey = 0, fstrMemberStatus = "Select Payee Status", fblnActive=true},
                new () {flngMemberStatusKey = 1, fstrMemberStatus = "Member", fblnActive=true},
                new () {flngMemberStatusKey = 2, fstrMemberStatus = "Partner", fblnActive=true},
                new () {flngMemberStatusKey = 3, fstrMemberStatus = "Visitor", fblnActive=true},
                new () {flngMemberStatusKey = 4, fstrMemberStatus = "Incoming Member", fblnActive=true},
            ];
    }

    public static IEnumerable<CollectionType> GetCollectionType()
    {
        return
            [
                new () {flngCollectionTypeKey = 0, fstrCollectionType = "Select Collection Type", fblnActive = true},
                new () {flngCollectionTypeKey = 1, fstrCollectionType = "Tithes", fblnActive = true},
                new () {flngCollectionTypeKey = 2, fstrCollectionType = "Offering", fblnActive = true},
                new () {flngCollectionTypeKey = 3, fstrCollectionType = "Sacrificial Seed", fblnActive = true},
                new () {flngCollectionTypeKey = 4, fstrCollectionType = "First Fruit", fblnActive = true},
                new () {flngCollectionTypeKey = 5, fstrCollectionType = "Building Fund", fblnActive = true},
                new () {flngCollectionTypeKey = 6, fstrCollectionType = "Partnership", fblnActive = true},
                new () {flngCollectionTypeKey = 7, fstrCollectionType = "Other", fblnActive = true}
            ];
    }

    public static IEnumerable<Roles> GetRoles()
    {
        return
            [
                new () {flngRoleKey = 1, fstrRoleName = "Administrator", fblnActive = true},
                new () {flngRoleKey = 2, fstrRoleName = "Manager", fblnActive = true},
                new () {flngRoleKey = 3, fstrRoleName = "User", fblnActive = true},
                new () {flngRoleKey = 4, fstrRoleName = "TempUser", fblnActive = true}
            ];
    }

    public static IEnumerable<Permission> GetPermissions()
    {
        return
            [
                // Dashboard
                new () {flngPermissionKey = 1, fstrPermission = "Dashboard.View", fstrDescription = "Dashboard View"},

                // Collection
                new () {flngPermissionKey = 2, fstrPermission = "Collection.View", fstrDescription = "Collection View"},
                new () {flngPermissionKey = 3, fstrPermission = "Collection.Edit", fstrDescription = "Collection Edit"},
                new () {flngPermissionKey = 4, fstrPermission = "Collection.Add", fstrDescription = "Collection Add"},

                // Report
                new () {flngPermissionKey = 7, fstrPermission = "Report.View", fstrDescription = "Report View"},
                new () {flngPermissionKey = 8, fstrPermission = "Report.Generate", fstrDescription = "Report Generate"},

                // Setting
                new () {flngPermissionKey = 9, fstrPermission = "AdminOnly", fstrDescription = "Admin Only"}
         
            ];
    }

    public static IEnumerable<CollectionNumber> GetCollectionNumber()
    {
        return
            [
                new () {flngCollectionNumberKey = 0, fstrCollectionNumberType = "Select a Collection Number"},
                new () {flngCollectionNumberKey = 1, fstrCollectionNumberType = "One"},
                new () {flngCollectionNumberKey = 2, fstrCollectionNumberType = "Two"},
                new () {flngCollectionNumberKey = 3, fstrCollectionNumberType = "Three"},
                new () {flngCollectionNumberKey = 4, fstrCollectionNumberType = "Four"},
                new () {flngCollectionNumberKey = 5, fstrCollectionNumberType = "Five"},
                new () {flngCollectionNumberKey = 6, fstrCollectionNumberType = "Six"},
                new () {flngCollectionNumberKey = 7, fstrCollectionNumberType = "Seven"},
                new () {flngCollectionNumberKey = 8, fstrCollectionNumberType = "Eight"},
                new () {flngCollectionNumberKey = 9, fstrCollectionNumberType = "Nine"},
                new () {flngCollectionNumberKey = 10, fstrCollectionNumberType = "Ten"}

            ];
    }

    public static IEnumerable<ReportOption> GetReportOptions()
    {
        return
            [
                new () {flngOptionKey = 1, fstrOptionType = "Detail Report"},
                new () { flngOptionKey = 2, fstrOptionType = "Payment Summary Report"},
                new() { flngOptionKey = 3, fstrOptionType = "Collection Type Summary Report"},
                new() { flngOptionKey = 4, fstrOptionType = "Monthly Summary Report"}
            ];
    }

    public static string DefaultPassword => "P@ssword";
}

public record PaymentType
{
    internal int flngPaymentKey { get; set; }
    internal string? fstrPaymentType { get; set; }
    internal bool fblnActive { get; set; }
}

public record MemberStatus
{
    internal int flngMemberStatusKey { get; set; }
    internal string? fstrMemberStatus { get; set; }
    internal bool fblnActive { get; set; }
}

public record CollectionType
{
    internal int flngCollectionTypeKey { get; set; }
    internal string? fstrCollectionType { get; set; }
    internal bool fblnActive { get; set; }
}

public record Roles
{
    internal int flngRoleKey { get; set; }
    internal string? fstrRoleName { get; set; }
    internal bool fblnActive { get; set; }
}

public record Permission
{
    internal int flngPermissionKey { get; set; }
    internal string? fstrPermission { get; set; }
    internal string? fstrDescription { get; set; }
}

public record CollectionNumber
{
    internal int flngCollectionNumberKey { get; set; }
    internal string? fstrCollectionNumberType { get; set; }

}

public record ReportOption
{
    internal int flngOptionKey { get; set; }
    internal string? fstrOptionType { get; set; }
}
