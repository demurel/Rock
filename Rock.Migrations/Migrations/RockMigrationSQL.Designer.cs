﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Rock.Migrations.Migrations {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class RockMigrationSQL {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal RockMigrationSQL() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Rock.Migrations.Migrations.RockMigrationSQL", typeof(RockMigrationSQL).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /* Pointer used for text / image updates. */
        ///DECLARE @pv binary(16)
        ///
        ///alter table [Attendance] nocheck constraint all
        ///alter table [AttendanceCode] nocheck constraint all
        ///alter table [Attribute] nocheck constraint all
        ///alter table [AttributeCategory] nocheck constraint all
        ///alter table [AttributeQualifier] nocheck constraint all
        ///alter table [AttributeValue] nocheck constraint all
        ///alter table [Audit] nocheck constraint all
        ///alter table [Auth] nocheck constraint all
        ///alter table [BinaryFile] nocheck cons [rest of string was truncated]&quot;;.
        /// </summary>
        public static string _201311251734059_CreateDatabase_PopulateData {
            get {
                return ResourceManager.GetString("_201311251734059_CreateDatabase_PopulateData", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 
        ///DECLARE @MergeTemplateEntityTypeId int = (SELECT TOP 1 [Id] FROM [EntityType] WHERE [Name] = &apos;Rock.MergeTemplates.WordDocumentMergeTemplateType&apos;)
        ///DECLARE @GeneralCategoryId int = (SELECT TOP 1 [Id] FROM [Category] WHERE [Guid] = &apos;CAA86576-901B-C4A6-4F62-70EB0A2B32A8&apos;)
        ///
        ///
        ///-- envelope
        ///INSERT INTO [dbo].[BinaryFile] ([IsTemporary], [IsSystem], [BinaryFileTypeId], [Path], [FileName], [MimeType], [Description], [StorageEntityTypeId], [Guid]) 
        ///        VALUES 
        ///        (0, 0, 3, N&apos;~/GetFile.ashx?guid=425298 [rest of string was truncated]&quot;;.
        /// </summary>
        public static string _201506051749054_AddSampleMergeDocs {
            get {
                return ResourceManager.GetString("_201506051749054_AddSampleMergeDocs", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to BEGIN TRY
        ///    ALTER TABLE Person
        ///
        ///    DROP COLUMN DaysUntilBirthday
        ///END TRY
        ///
        ///BEGIN CATCH
        ///END CATCH
        ///
        ///ALTER TABLE Person ADD DaysUntilBirthday AS (
        ///    CASE 
        ///        -- if there birthday is Feb 29 and their next birthday is this year and it isn&apos;t a leap year, set their birthday to Feb 28 (this year)
        ///        WHEN (
        ///                BirthMonth = 2
        ///                AND BirthDay = 29
        ///                AND datepart(month, sysdatetime()) &lt; 3
        ///                AND (isdate(convert(VARCHAR(4), datepart(year, [rest of string was truncated]&quot;;.
        /// </summary>
        public static string _201506112030400_PersonDaysUntilBirthday {
            get {
                return ResourceManager.GetString("_201506112030400_PersonDaysUntilBirthday", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /*
        ///&lt;doc&gt;
        ///	&lt;summary&gt;
        ///		This stored procedure returns data used by the pledge analytics block
        ///	&lt;/summary&gt;
        ///&lt;/doc&gt;
        ///*/
        ///CREATE PROCEDURE [dbo].[spFinance_PledgeAnalyticsQuery]
        ///	  @AccountId int
        ///	, @StartDate datetime = NULL
        ///	, @EndDate datetime = NULL
        ///	, @MinAmountPledged decimal(18,2) = NULL
        ///	, @MaxAmountPledged decimal(18,2) = NULL
        ///	, @MinComplete decimal(18,2) = NULL
        ///	, @MaxComplete decimal(18,2) = NULL
        ///	, @MinAmountGiven decimal(18,2) = NULL
        ///	, @MaxAmountGiven decimal(18,2) = NULL
        ///	, @Include [rest of string was truncated]&quot;;.
        /// </summary>
        public static string _201506121300596_FinancialAnalyticsProcs_1 {
            get {
                return ResourceManager.GetString("_201506121300596_FinancialAnalyticsProcs_1", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /*
        ///&lt;doc&gt;
        ///	&lt;summary&gt;
        ///		This stored procedure returns data used by the giving analytics block
        ///	&lt;/summary&gt;
        ///&lt;/doc&gt;
        ///*/
        ///CREATE PROCEDURE [dbo].[spFinance_GivingAnalyticsQuery]
        ///	  @StartDate datetime = NULL
        ///	, @EndDate datetime = NULL
        ///	, @MinAmount decimal(18,2) = NULL
        ///	, @MaxAmount decimal(18,2) = NULL
        ///	, @AccountIds varchar(max) = NULL
        ///	, @CurrencyTypeIds varchar(max) = NULL
        ///	, @SourceTypeIds varchar(max) = NULL
        ///	, @ViewBy varchar(1) = &apos;G&apos;		-- G = Giving Leader, A = Adults, C = Children, F = Famil [rest of string was truncated]&quot;;.
        /// </summary>
        public static string _201506121300596_FinancialAnalyticsProcs_2 {
            get {
                return ResourceManager.GetString("_201506121300596_FinancialAnalyticsProcs_2", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ALTER TABLE AttributeValue
        ///
        ///DROP COLUMN ValueAsNumeric
        ///
        ///ALTER TABLE AttributeValue
        ///
        ///DROP COLUMN ValueAsDateTime
        ///
        ///ALTER TABLE AttributeValue ADD [ValueAsNumeric] AS (
        ///    CASE 
        ///        WHEN len([value]) &lt; (100)
        ///            AND isnumeric([value]) = (1)
        ///            AND NOT [value] LIKE &apos;%[^0-9.]%&apos;
        ///            AND NOT [value] LIKE &apos;%[.]%&apos;
        ///            THEN TRY_CONVERT([numeric](38, 10), [value])
        ///        END
        ///    ) persisted
        ///
        ///ALTER TABLE AttributeValue ADD [ValueAsDateTime] AS ( 
        ///    CASE WHEN i [rest of string was truncated]&quot;;.
        /// </summary>
        public static string _201506130028565_AttributeValueAsDateTimeIndex_Create {
            get {
                return ResourceManager.GetString("_201506130028565_AttributeValueAsDateTimeIndex_Create", resourceCulture);
            }
        }
    }
}
