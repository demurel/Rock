// <copyright>
// Copyright 2013 by the Spark Development Network
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//
namespace Rock.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    /// <summary>
    ///
    /// </summary>
    public partial class EventRegistration : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            CreateTable(
                "dbo.RegistrationRegistrant",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegistrationId = c.Int(nullable: false),
                        PersonAliasId = c.Int(),
                        GroupMemberId = c.Int(),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedDateTime = c.DateTime(),
                        ModifiedDateTime = c.DateTime(),
                        CreatedByPersonAliasId = c.Int(),
                        ModifiedByPersonAliasId = c.Int(),
                        Guid = c.Guid(nullable: false),
                        ForeignId = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PersonAlias", t => t.CreatedByPersonAliasId)
                .ForeignKey("dbo.GroupMember", t => t.GroupMemberId)
                .ForeignKey("dbo.PersonAlias", t => t.ModifiedByPersonAliasId)
                .ForeignKey("dbo.PersonAlias", t => t.PersonAliasId)
                .ForeignKey("dbo.Registration", t => t.RegistrationId, cascadeDelete: true)
                .Index(t => t.RegistrationId)
                .Index(t => t.PersonAliasId)
                .Index(t => t.GroupMemberId)
                .Index(t => t.CreatedByPersonAliasId)
                .Index(t => t.ModifiedByPersonAliasId)
                .Index(t => t.Guid, unique: true)
                .Index(t => t.ForeignId);
            
            CreateTable(
                "dbo.RegistrationRegistrantFee",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegistrationRegistrantId = c.Int(nullable: false),
                        RegistrationTemplateFeeId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedDateTime = c.DateTime(),
                        ModifiedDateTime = c.DateTime(),
                        CreatedByPersonAliasId = c.Int(),
                        ModifiedByPersonAliasId = c.Int(),
                        Guid = c.Guid(nullable: false),
                        ForeignId = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PersonAlias", t => t.CreatedByPersonAliasId)
                .ForeignKey("dbo.PersonAlias", t => t.ModifiedByPersonAliasId)
                .ForeignKey("dbo.RegistrationRegistrant", t => t.RegistrationRegistrantId, cascadeDelete: true)
                .ForeignKey("dbo.RegistrationTemplateFee", t => t.RegistrationTemplateFeeId)
                .Index(t => t.RegistrationRegistrantId)
                .Index(t => t.RegistrationTemplateFeeId)
                .Index(t => t.CreatedByPersonAliasId)
                .Index(t => t.ModifiedByPersonAliasId)
                .Index(t => t.Guid, unique: true)
                .Index(t => t.ForeignId);
            
            CreateTable(
                "dbo.RegistrationTemplateFee",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        RegistrationTemplateId = c.Int(nullable: false),
                        FeeType = c.Int(nullable: false),
                        CostValue = c.String(maxLength: 400),
                        DiscountApplies = c.Boolean(nullable: false),
                        AllowMultiple = c.Boolean(nullable: false),
                        Order = c.Int(nullable: false),
                        CreatedDateTime = c.DateTime(),
                        ModifiedDateTime = c.DateTime(),
                        CreatedByPersonAliasId = c.Int(),
                        ModifiedByPersonAliasId = c.Int(),
                        Guid = c.Guid(nullable: false),
                        ForeignId = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PersonAlias", t => t.CreatedByPersonAliasId)
                .ForeignKey("dbo.PersonAlias", t => t.ModifiedByPersonAliasId)
                .ForeignKey("dbo.RegistrationTemplate", t => t.RegistrationTemplateId, cascadeDelete: true)
                .Index(t => t.RegistrationTemplateId)
                .Index(t => t.CreatedByPersonAliasId)
                .Index(t => t.ModifiedByPersonAliasId)
                .Index(t => t.Guid, unique: true)
                .Index(t => t.ForeignId);
            
            CreateTable(
                "dbo.RegistrationTemplate",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        CategoryId = c.Int(),
                        GroupTypeId = c.Int(),
                        GroupMemberRoleId = c.Int(),
                        GroupMemberStatus = c.Int(nullable: false),
                        NotifyGroupLeaders = c.Boolean(nullable: false),
                        FeeTerm = c.String(maxLength: 100),
                        RegistrantTerm = c.String(maxLength: 100),
                        RegistrationTerm = c.String(maxLength: 100),
                        DiscountCodeTerm = c.String(maxLength: 100),
                        UseDefaultConfirmationEmail = c.Boolean(nullable: false),
                        ConfirmationEmailTemplate = c.String(),
                        ReminderEmailTemplate = c.String(),
                        Cost = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MinimumInitialPayment = c.Decimal(nullable: false, precision: 18, scale: 2),
                        LoginRequired = c.Boolean(nullable: false),
                        RegistrantsSameFamily = c.Int(nullable: false),
                        RequestEntryName = c.String(),
                        SuccessTitle = c.String(),
                        SuccessText = c.String(),
                        AllowMultipleRegistrants = c.Boolean(nullable: false),
                        MaxRegistrants = c.Int(nullable: false),
                        FinancialGatewayId = c.Int(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedDateTime = c.DateTime(),
                        ModifiedDateTime = c.DateTime(),
                        CreatedByPersonAliasId = c.Int(),
                        ModifiedByPersonAliasId = c.Int(),
                        Guid = c.Guid(nullable: false),
                        ForeignId = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Category", t => t.CategoryId)
                .ForeignKey("dbo.PersonAlias", t => t.CreatedByPersonAliasId)
                .ForeignKey("dbo.FinancialGateway", t => t.FinancialGatewayId)
                .ForeignKey("dbo.GroupType", t => t.GroupTypeId)
                .ForeignKey("dbo.PersonAlias", t => t.ModifiedByPersonAliasId)
                .Index(t => t.CategoryId)
                .Index(t => t.GroupTypeId)
                .Index(t => t.FinancialGatewayId)
                .Index(t => t.CreatedByPersonAliasId)
                .Index(t => t.ModifiedByPersonAliasId)
                .Index(t => t.Guid, unique: true)
                .Index(t => t.ForeignId);
            
            CreateTable(
                "dbo.RegistrationTemplateDiscount",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Code = c.String(nullable: false, maxLength: 100),
                        RegistrationTemplateId = c.Int(nullable: false),
                        DiscountPercentage = c.Double(nullable: false),
                        DiscountAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Order = c.Int(nullable: false),
                        CreatedDateTime = c.DateTime(),
                        ModifiedDateTime = c.DateTime(),
                        CreatedByPersonAliasId = c.Int(),
                        ModifiedByPersonAliasId = c.Int(),
                        Guid = c.Guid(nullable: false),
                        ForeignId = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PersonAlias", t => t.CreatedByPersonAliasId)
                .ForeignKey("dbo.PersonAlias", t => t.ModifiedByPersonAliasId)
                .ForeignKey("dbo.RegistrationTemplate", t => t.RegistrationTemplateId, cascadeDelete: true)
                .Index(t => t.RegistrationTemplateId)
                .Index(t => t.CreatedByPersonAliasId)
                .Index(t => t.ModifiedByPersonAliasId)
                .Index(t => t.Guid, unique: true)
                .Index(t => t.ForeignId);
            
            CreateTable(
                "dbo.RegistrationTemplateForm",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        RegistrationTemplateId = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        CreatedDateTime = c.DateTime(),
                        ModifiedDateTime = c.DateTime(),
                        CreatedByPersonAliasId = c.Int(),
                        ModifiedByPersonAliasId = c.Int(),
                        Guid = c.Guid(nullable: false),
                        ForeignId = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PersonAlias", t => t.CreatedByPersonAliasId)
                .ForeignKey("dbo.PersonAlias", t => t.ModifiedByPersonAliasId)
                .ForeignKey("dbo.RegistrationTemplate", t => t.RegistrationTemplateId, cascadeDelete: true)
                .Index(t => t.RegistrationTemplateId)
                .Index(t => t.CreatedByPersonAliasId)
                .Index(t => t.ModifiedByPersonAliasId)
                .Index(t => t.Guid, unique: true)
                .Index(t => t.ForeignId);
            
            CreateTable(
                "dbo.RegistrationTemplateFormField",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegistrationTemplateFormId = c.Int(nullable: false),
                        FieldSource = c.Int(nullable: false),
                        PersonFieldType = c.Int(nullable: false),
                        AttributeId = c.Int(),
                        IsSharedValue = c.Boolean(nullable: false),
                        ShowCurrentValue = c.Boolean(nullable: false),
                        PreText = c.String(),
                        PostText = c.String(),
                        IsGridField = c.Boolean(nullable: false),
                        IsRequired = c.Boolean(nullable: false),
                        Order = c.Int(nullable: false),
                        CreatedDateTime = c.DateTime(),
                        ModifiedDateTime = c.DateTime(),
                        CreatedByPersonAliasId = c.Int(),
                        ModifiedByPersonAliasId = c.Int(),
                        Guid = c.Guid(nullable: false),
                        ForeignId = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Attribute", t => t.AttributeId)
                .ForeignKey("dbo.PersonAlias", t => t.CreatedByPersonAliasId)
                .ForeignKey("dbo.PersonAlias", t => t.ModifiedByPersonAliasId)
                .ForeignKey("dbo.RegistrationTemplateForm", t => t.RegistrationTemplateFormId, cascadeDelete: true)
                .Index(t => t.RegistrationTemplateFormId)
                .Index(t => t.AttributeId)
                .Index(t => t.CreatedByPersonAliasId)
                .Index(t => t.ModifiedByPersonAliasId)
                .Index(t => t.Guid, unique: true)
                .Index(t => t.ForeignId);
            
            CreateTable(
                "dbo.RegistrationInstance",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        RegistrationTemplateId = c.Int(nullable: false),
                        StartDateTime = c.DateTime(),
                        EndDateTime = c.DateTime(),
                        Details = c.String(),
                        MaxAttendees = c.Int(nullable: false),
                        AccountId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        ContactName = c.String(maxLength: 200),
                        ContactEmail = c.String(maxLength: 200),
                        AdditionalReminderDetails = c.String(),
                        AdditionalConfirmationDetails = c.String(),
                        ReminderSentDateTime = c.DateTime(),
                        ConfirmationSentDateTime = c.DateTime(),
                        CreatedDateTime = c.DateTime(),
                        ModifiedDateTime = c.DateTime(),
                        CreatedByPersonAliasId = c.Int(),
                        ModifiedByPersonAliasId = c.Int(),
                        Guid = c.Guid(nullable: false),
                        ForeignId = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FinancialAccount", t => t.AccountId)
                .ForeignKey("dbo.PersonAlias", t => t.CreatedByPersonAliasId)
                .ForeignKey("dbo.PersonAlias", t => t.ModifiedByPersonAliasId)
                .ForeignKey("dbo.RegistrationTemplate", t => t.RegistrationTemplateId, cascadeDelete: true)
                .Index(t => t.RegistrationTemplateId)
                .Index(t => t.AccountId)
                .Index(t => t.CreatedByPersonAliasId)
                .Index(t => t.ModifiedByPersonAliasId)
                .Index(t => t.Guid, unique: true)
                .Index(t => t.ForeignId);
            
            CreateTable(
                "dbo.Registration",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        RegistrationInstanceId = c.Int(nullable: false),
                        PersonAliasId = c.Int(),
                        FirstName = c.String(maxLength: 50),
                        LastName = c.String(maxLength: 50),
                        ConfirmationEmail = c.String(maxLength: 75),
                        GroupId = c.Int(),
                        CreatedDateTime = c.DateTime(),
                        ModifiedDateTime = c.DateTime(),
                        CreatedByPersonAliasId = c.Int(),
                        ModifiedByPersonAliasId = c.Int(),
                        Guid = c.Guid(nullable: false),
                        ForeignId = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.PersonAlias", t => t.CreatedByPersonAliasId)
                .ForeignKey("dbo.Group", t => t.GroupId)
                .ForeignKey("dbo.PersonAlias", t => t.ModifiedByPersonAliasId)
                .ForeignKey("dbo.PersonAlias", t => t.PersonAliasId)
                .ForeignKey("dbo.RegistrationInstance", t => t.RegistrationInstanceId)
                .Index(t => t.RegistrationInstanceId)
                .Index(t => t.PersonAliasId)
                .Index(t => t.GroupId)
                .Index(t => t.CreatedByPersonAliasId)
                .Index(t => t.ModifiedByPersonAliasId)
                .Index(t => t.Guid, unique: true)
                .Index(t => t.ForeignId);

            Sql( @"
    DECLARE @MaxAccountOrder int = ( SELECT TOP 1 [Order] FROM [FinancialAccount] WHERE [ParentAccountId] IS NULL )
    INSERT INTO [dbo].[FinancialAccount] ( [Name], [IsTaxDeductible], [Order], [IsActive], [Guid]) 
    VALUES ( 'Event Registration', 0, COALESCE(@MaxAccountOrder + 1, 0), 1, '2A6F9E5F-6859-44F1-AB0E-CE9CF6B08EE5')
" );
        }
        
        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            Sql( @"
    DELETE [dbo].[FinancialAccount] WHERE [Guid] = '2A6F9E5F-6859-44F1-AB0E-CE9CF6B08EE5'
" );

            DropForeignKey("dbo.RegistrationRegistrant", "RegistrationId", "dbo.Registration");
            DropForeignKey("dbo.RegistrationRegistrant", "PersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationRegistrant", "ModifiedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationRegistrant", "GroupMemberId", "dbo.GroupMember");
            DropForeignKey("dbo.RegistrationRegistrantFee", "RegistrationTemplateFeeId", "dbo.RegistrationTemplateFee");
            DropForeignKey("dbo.RegistrationTemplateFee", "RegistrationTemplateId", "dbo.RegistrationTemplate");
            DropForeignKey("dbo.RegistrationTemplate", "ModifiedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationInstance", "RegistrationTemplateId", "dbo.RegistrationTemplate");
            DropForeignKey("dbo.Registration", "RegistrationInstanceId", "dbo.RegistrationInstance");
            DropForeignKey("dbo.Registration", "PersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.Registration", "ModifiedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.Registration", "GroupId", "dbo.Group");
            DropForeignKey("dbo.Registration", "CreatedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationInstance", "ModifiedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationInstance", "CreatedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationInstance", "AccountId", "dbo.FinancialAccount");
            DropForeignKey("dbo.RegistrationTemplate", "GroupTypeId", "dbo.GroupType");
            DropForeignKey("dbo.RegistrationTemplateForm", "RegistrationTemplateId", "dbo.RegistrationTemplate");
            DropForeignKey("dbo.RegistrationTemplateForm", "ModifiedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationTemplateFormField", "RegistrationTemplateFormId", "dbo.RegistrationTemplateForm");
            DropForeignKey("dbo.RegistrationTemplateFormField", "ModifiedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationTemplateFormField", "CreatedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationTemplateFormField", "AttributeId", "dbo.Attribute");
            DropForeignKey("dbo.RegistrationTemplateForm", "CreatedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationTemplate", "FinancialGatewayId", "dbo.FinancialGateway");
            DropForeignKey("dbo.RegistrationTemplateDiscount", "RegistrationTemplateId", "dbo.RegistrationTemplate");
            DropForeignKey("dbo.RegistrationTemplateDiscount", "ModifiedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationTemplateDiscount", "CreatedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationTemplate", "CreatedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationTemplate", "CategoryId", "dbo.Category");
            DropForeignKey("dbo.RegistrationTemplateFee", "ModifiedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationTemplateFee", "CreatedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationRegistrantFee", "RegistrationRegistrantId", "dbo.RegistrationRegistrant");
            DropForeignKey("dbo.RegistrationRegistrantFee", "ModifiedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationRegistrantFee", "CreatedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationRegistrant", "CreatedByPersonAliasId", "dbo.PersonAlias");
            DropIndex("dbo.Registration", new[] { "ForeignId" });
            DropIndex("dbo.Registration", new[] { "Guid" });
            DropIndex("dbo.Registration", new[] { "ModifiedByPersonAliasId" });
            DropIndex("dbo.Registration", new[] { "CreatedByPersonAliasId" });
            DropIndex("dbo.Registration", new[] { "GroupId" });
            DropIndex("dbo.Registration", new[] { "PersonAliasId" });
            DropIndex("dbo.Registration", new[] { "RegistrationInstanceId" });
            DropIndex("dbo.RegistrationInstance", new[] { "ForeignId" });
            DropIndex("dbo.RegistrationInstance", new[] { "Guid" });
            DropIndex("dbo.RegistrationInstance", new[] { "ModifiedByPersonAliasId" });
            DropIndex("dbo.RegistrationInstance", new[] { "CreatedByPersonAliasId" });
            DropIndex("dbo.RegistrationInstance", new[] { "AccountId" });
            DropIndex("dbo.RegistrationInstance", new[] { "RegistrationTemplateId" });
            DropIndex("dbo.RegistrationTemplateFormField", new[] { "ForeignId" });
            DropIndex("dbo.RegistrationTemplateFormField", new[] { "Guid" });
            DropIndex("dbo.RegistrationTemplateFormField", new[] { "ModifiedByPersonAliasId" });
            DropIndex("dbo.RegistrationTemplateFormField", new[] { "CreatedByPersonAliasId" });
            DropIndex("dbo.RegistrationTemplateFormField", new[] { "AttributeId" });
            DropIndex("dbo.RegistrationTemplateFormField", new[] { "RegistrationTemplateFormId" });
            DropIndex("dbo.RegistrationTemplateForm", new[] { "ForeignId" });
            DropIndex("dbo.RegistrationTemplateForm", new[] { "Guid" });
            DropIndex("dbo.RegistrationTemplateForm", new[] { "ModifiedByPersonAliasId" });
            DropIndex("dbo.RegistrationTemplateForm", new[] { "CreatedByPersonAliasId" });
            DropIndex("dbo.RegistrationTemplateForm", new[] { "RegistrationTemplateId" });
            DropIndex("dbo.RegistrationTemplateDiscount", new[] { "ForeignId" });
            DropIndex("dbo.RegistrationTemplateDiscount", new[] { "Guid" });
            DropIndex("dbo.RegistrationTemplateDiscount", new[] { "ModifiedByPersonAliasId" });
            DropIndex("dbo.RegistrationTemplateDiscount", new[] { "CreatedByPersonAliasId" });
            DropIndex("dbo.RegistrationTemplateDiscount", new[] { "RegistrationTemplateId" });
            DropIndex("dbo.RegistrationTemplate", new[] { "ForeignId" });
            DropIndex("dbo.RegistrationTemplate", new[] { "Guid" });
            DropIndex("dbo.RegistrationTemplate", new[] { "ModifiedByPersonAliasId" });
            DropIndex("dbo.RegistrationTemplate", new[] { "CreatedByPersonAliasId" });
            DropIndex("dbo.RegistrationTemplate", new[] { "FinancialGatewayId" });
            DropIndex("dbo.RegistrationTemplate", new[] { "GroupTypeId" });
            DropIndex("dbo.RegistrationTemplate", new[] { "CategoryId" });
            DropIndex("dbo.RegistrationTemplateFee", new[] { "ForeignId" });
            DropIndex("dbo.RegistrationTemplateFee", new[] { "Guid" });
            DropIndex("dbo.RegistrationTemplateFee", new[] { "ModifiedByPersonAliasId" });
            DropIndex("dbo.RegistrationTemplateFee", new[] { "CreatedByPersonAliasId" });
            DropIndex("dbo.RegistrationTemplateFee", new[] { "RegistrationTemplateId" });
            DropIndex("dbo.RegistrationRegistrantFee", new[] { "ForeignId" });
            DropIndex("dbo.RegistrationRegistrantFee", new[] { "Guid" });
            DropIndex("dbo.RegistrationRegistrantFee", new[] { "ModifiedByPersonAliasId" });
            DropIndex("dbo.RegistrationRegistrantFee", new[] { "CreatedByPersonAliasId" });
            DropIndex("dbo.RegistrationRegistrantFee", new[] { "RegistrationTemplateFeeId" });
            DropIndex("dbo.RegistrationRegistrantFee", new[] { "RegistrationRegistrantId" });
            DropIndex("dbo.RegistrationRegistrant", new[] { "ForeignId" });
            DropIndex("dbo.RegistrationRegistrant", new[] { "Guid" });
            DropIndex("dbo.RegistrationRegistrant", new[] { "ModifiedByPersonAliasId" });
            DropIndex("dbo.RegistrationRegistrant", new[] { "CreatedByPersonAliasId" });
            DropIndex("dbo.RegistrationRegistrant", new[] { "GroupMemberId" });
            DropIndex("dbo.RegistrationRegistrant", new[] { "PersonAliasId" });
            DropIndex("dbo.RegistrationRegistrant", new[] { "RegistrationId" });
            DropTable("dbo.Registration");
            DropTable("dbo.RegistrationInstance");
            DropTable("dbo.RegistrationTemplateFormField");
            DropTable("dbo.RegistrationTemplateForm");
            DropTable("dbo.RegistrationTemplateDiscount");
            DropTable("dbo.RegistrationTemplate");
            DropTable("dbo.RegistrationTemplateFee");
            DropTable("dbo.RegistrationRegistrantFee");
            DropTable("dbo.RegistrationRegistrant");
        }
    }
}
