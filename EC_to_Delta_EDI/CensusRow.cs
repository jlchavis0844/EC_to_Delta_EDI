// <copyright file="CensusRow.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using CsvHelper.Configuration;

public class CensusRow {
    public string CompanyName { get; set; }

    public string EID { get; set; }

    public string Location { get; set; }

    public string FirstName { get; set; }

    public string MiddleName { get; set; }

    public string LastName { get; set; }

    public string Relationship { get; set; }

    public string RelationshipCode { get; set; }

    public string SSN { get; set; }

    public string Gender { get; set; }

    public string BirthDate { get; set; }

    public string Race { get; set; }

    public string Citizenship { get; set; }

    public string Address1 { get; set; }

    public string Address2 { get; set; }

    public string City { get; set; }

    public string State { get; set; }

    public string Zip { get; set; }

    public string County { get; set; }

    public string Country { get; set; }

    public string PersonalPhone { get; set; }

    public string WorkPhone { get; set; }

    public string MobilePhone { get; set; }

    public string Email { get; set; }

    public string PersonalEmail { get; set; }

    public string EmployeeType { get; set; }

    public string EmployeeStatus { get; set; }

    public string HireDate { get; set; }

    public string TerminationDate { get; set; }

    public string Department { get; set; }

    public string Division { get; set; }

    public string JobClass { get; set; }

    public string JobTitle { get; set; }

    public string MaritalStatus { get; set; }

    public string MaritalDate { get; set; }

    public string MaritalLocation { get; set; }

    public string StudentStatus { get; set; }

    public string ScheduledHours { get; set; }

    public string SickHours { get; set; }

    public string PersonalHours { get; set; }

    public string W2Wages { get; set; }

    public string Compensation { get; set; }

    public string CompensationType { get; set; }

    public string PayCycle { get; set; }

    public string PayPeriods { get; set; }

    public string CostFactor { get; set; }

    public string TobaccoUser { get; set; }

    public string Disabled { get; set; }

    public string MedicareADate { get; set; }

    public string MedicareBDate { get; set; }

    public string MedicareCDate { get; set; }

    public string MedicareDDate { get; set; }

    public string MedicalPCPName { get; set; }

    public string MedicalPCPID { get; set; }

    public string DentalPCPName { get; set; }

    public string DentalPCPID { get; set; }

    public string IPANumber { get; set; }

    public string OBGYN { get; set; }

    public string BenefitEligibleDate { get; set; }

    public string UnlockEnrollmentDate { get; set; }

    public string OriginalEffectiveDateInfo { get; set; }

    public string SubscriberKey { get; set; }

    public string PlanType { get; set; }

    public string PlanEffectiveStartDate { get; set; }

    public string PlanEffectiveEndDate { get; set; }

    public string PlanAdminName { get; set; }

    public string PlanDisplayName { get; set; }

    public string PlanImportID { get; set; }

    public string EffectiveDate { get; set; }

    public string CoverageDetails { get; set; }

    public string ElectionStatus { get; set; }

    public string RiderCodes { get; set; }

    public string Action { get; set; }

    public string WaiveReason { get; set; }

    public string PolicyNumber { get; set; }

    public string SubgroupNumber { get; set; }

    public string AgeDetermination { get; set; }

    public string Carrier { get; set; }

    public string TotalRate { get; set; }

    public string EmployeeRate { get; set; }

    public string SpouseRate { get; set; }

    public string ChildrenRate { get; set; }

    public string EmployeeContribution { get; set; }

    public string EmployeePre_TaxCost { get; set; }

    public string EmployeePost_TaxCost { get; set; }

    public string EmployeeCostPerDeductionPeriod { get; set; }

    public string PlanDeductionCycle { get; set; }

    public string LastModifiedDate { get; set; }

    public string LastModifiedBy { get; set; }

    public string E_SignDate { get; set; }

    public string CalPERS_ID { get; set; }

    // public string EnrolledBy { get; set; }
    // public string NewBusiness { get; set; }

    // override to print all frields in a CensusRow

    /// <inheritdoc/>
    public override string ToString() {
        string retStr = this.CompanyName + " | " + this.EID + " | " + this.Location + " | " + this.FirstName + " | " +
            this.MiddleName + " | " + this.LastName + " | " + this.Relationship + " | " + this.RelationshipCode + " | " +
            this.SSN + " | " + this.Gender + " | " + this.BirthDate + " | " + this.Race + " | " + this.Citizenship + " | " +
            this.Address1 + " | " + this.Address2 + " | " + this.City + " | " + this.State + " | " + this.Zip + " | " +
            this.County + " | " + this.Country + " | " + this.PersonalPhone + " | " + this.WorkPhone + " | " +
            this.MobilePhone + " | " + this.Email + " | " + this.PersonalEmail + " | " + this.EmployeeType + " | " +
            this.EmployeeStatus + " | " + this.HireDate + " | " + this.TerminationDate + " | " + this.Department + " | " +
            this.Division + " | " + this.JobClass + " | " + this.JobTitle + " | " + this.MaritalStatus + " | " + this.MaritalDate + " | " +
            this.MaritalLocation + " | " + this.StudentStatus + " | " + this.ScheduledHours + " | " + this.SickHours + " | " +
            this.PersonalHours + " | " + this.W2Wages + " | " + this.Compensation + " | " + this.CompensationType + " | " +
            this.PayCycle + " | " + this.PayPeriods + " | " + this.CostFactor + " | " + this.TobaccoUser + " | " + this.Disabled + " | " +
            this.MedicareADate + " | " + this.MedicareBDate + " | " + this.MedicareCDate + " | " + this.MedicareDDate + " | " +
            this.MedicalPCPName + " | " + this.MedicalPCPID + " | " + this.DentalPCPName + " | " + this.DentalPCPID + " | " +
            this.IPANumber + " | " + this.OBGYN + " | " + this.BenefitEligibleDate + " | " + this.UnlockEnrollmentDate + " | " +
            this.OriginalEffectiveDateInfo + " | " + this.SubscriberKey + " | " + this.PlanType + " | " + this.PlanEffectiveStartDate + " | " +
            this.PlanEffectiveEndDate + " | " + this.PlanAdminName + " | " + this.PlanDisplayName + " | " + this.PlanImportID + " | " +
            this.EffectiveDate + " | " + this.CoverageDetails + " | " + this.ElectionStatus + " | " + this.RiderCodes + " | " +
            this.Action + " | " + this.WaiveReason + " | " + this.PolicyNumber + " | " + this.SubgroupNumber + " | " +
            this.AgeDetermination + " | " + this.Carrier + " | " + this.TotalRate + " | " + this.EmployeeRate + " | " +
            this.SpouseRate + " | " + this.ChildrenRate + " | " + this.EmployeeContribution + " | " + this.EmployeePre_TaxCost + " | " +
            this.EmployeePost_TaxCost + " | " + this.EmployeeCostPerDeductionPeriod + " | " + this.PlanDeductionCycle + " | " +
            this.LastModifiedDate + " | " + this.LastModifiedBy + " | " + this.E_SignDate + " | " + this.CalPERS_ID;
        return retStr.Replace("  ", " ");
    }
}
