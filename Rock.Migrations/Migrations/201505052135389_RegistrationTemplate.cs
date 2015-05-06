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
    public partial class RegistrationTemplate : Rock.Migrations.RockMigration
    {
        /// <summary>
        /// Operations to be performed during the upgrade process.
        /// </summary>
        public override void Up()
        {
            CreateTable(
                "dbo.RegistrationTemplateFee",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        RegistrationTemplateId = c.Int(nullable: false),
                        FeeType = c.Int(nullable: false),
                        CostValue = c.String(),
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
                        RequestHomeCampus = c.Int(nullable: false),
                        RequestPhone = c.Int(nullable: false),
                        RequestHomeAddress = c.Int(nullable: false),
                        RequestEmail = c.Int(nullable: false),
                        RequestBirthDate = c.Int(nullable: false),
                        RequestGender = c.Int(nullable: false),
                        RequestMaritalStatus = c.Int(nullable: false),
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
                "dbo.RegistrationTemplateFormAttribute",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        RegistrationTemplateFormId = c.Int(nullable: false),
                        AppliesTo = c.Int(nullable: false),
                        AttributeId = c.Int(nullable: false),
                        CommonValue = c.Boolean(nullable: false),
                        UseCurrentPersonAttributeValue = c.Boolean(nullable: false),
                        PreText = c.String(),
                        PostText = c.String(),
                        ShowOnGrid = c.Boolean(nullable: false),
                        RequiredOnInitialEntry = c.Boolean(nullable: false),
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
                        AccountCode = c.String(maxLength: 100),
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
                .ForeignKey("dbo.PersonAlias", t => t.CreatedByPersonAliasId)
                .ForeignKey("dbo.PersonAlias", t => t.ModifiedByPersonAliasId)
                .ForeignKey("dbo.RegistrationTemplate", t => t.RegistrationTemplateId, cascadeDelete: true)
                .Index(t => t.RegistrationTemplateId)
                .Index(t => t.CreatedByPersonAliasId)
                .Index(t => t.ModifiedByPersonAliasId)
                .Index(t => t.Guid, unique: true)
                .Index(t => t.ForeignId);
            
        }
        
        /// <summary>
        /// Operations to be performed during the downgrade process.
        /// </summary>
        public override void Down()
        {
            DropForeignKey("dbo.RegistrationTemplateFee", "RegistrationTemplateId", "dbo.RegistrationTemplate");
            DropForeignKey("dbo.RegistrationTemplate", "ModifiedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationInstance", "RegistrationTemplateId", "dbo.RegistrationTemplate");
            DropForeignKey("dbo.RegistrationInstance", "ModifiedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationInstance", "CreatedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationTemplate", "GroupTypeId", "dbo.GroupType");
            DropForeignKey("dbo.RegistrationTemplateForm", "RegistrationTemplateId", "dbo.RegistrationTemplate");
            DropForeignKey("dbo.RegistrationTemplateForm", "ModifiedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationTemplateFormAttribute", "RegistrationTemplateFormId", "dbo.RegistrationTemplateForm");
            DropForeignKey("dbo.RegistrationTemplateFormAttribute", "ModifiedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationTemplateFormAttribute", "CreatedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationTemplateFormAttribute", "AttributeId", "dbo.Attribute");
            DropForeignKey("dbo.RegistrationTemplateForm", "CreatedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationTemplate", "FinancialGatewayId", "dbo.FinancialGateway");
            DropForeignKey("dbo.RegistrationTemplateDiscount", "RegistrationTemplateId", "dbo.RegistrationTemplate");
            DropForeignKey("dbo.RegistrationTemplateDiscount", "ModifiedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationTemplateDiscount", "CreatedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationTemplate", "CreatedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationTemplate", "CategoryId", "dbo.Category");
            DropForeignKey("dbo.RegistrationTemplateFee", "ModifiedByPersonAliasId", "dbo.PersonAlias");
            DropForeignKey("dbo.RegistrationTemplateFee", "CreatedByPersonAliasId", "dbo.PersonAlias");
            DropIndex("dbo.RegistrationInstance", new[] { "ForeignId" });
            DropIndex("dbo.RegistrationInstance", new[] { "Guid" });
            DropIndex("dbo.RegistrationInstance", new[] { "ModifiedByPersonAliasId" });
            DropIndex("dbo.RegistrationInstance", new[] { "CreatedByPersonAliasId" });
            DropIndex("dbo.RegistrationInstance", new[] { "RegistrationTemplateId" });
            DropIndex("dbo.RegistrationTemplateFormAttribute", new[] { "ForeignId" });
            DropIndex("dbo.RegistrationTemplateFormAttribute", new[] { "Guid" });
            DropIndex("dbo.RegistrationTemplateFormAttribute", new[] { "ModifiedByPersonAliasId" });
            DropIndex("dbo.RegistrationTemplateFormAttribute", new[] { "CreatedByPersonAliasId" });
            DropIndex("dbo.RegistrationTemplateFormAttribute", new[] { "AttributeId" });
            DropIndex("dbo.RegistrationTemplateFormAttribute", new[] { "RegistrationTemplateFormId" });
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
            DropTable("dbo.RegistrationInstance");
            DropTable("dbo.RegistrationTemplateFormAttribute");
            DropTable("dbo.RegistrationTemplateForm");
            DropTable("dbo.RegistrationTemplateDiscount");
            DropTable("dbo.RegistrationTemplate");
            DropTable("dbo.RegistrationTemplateFee");
        }
    }
}
