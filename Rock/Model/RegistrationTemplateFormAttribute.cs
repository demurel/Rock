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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Runtime.Serialization;

using Newtonsoft.Json;

using Rock.Data;
using Rock.Security;

namespace Rock.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Table( "RegistrationTemplateFormAttribute" )]
    [DataContract]
    public partial class RegistrationTemplateFormAttribute : Model<RegistrationTemplateFormAttribute>
    {

        #region Entity Properties

        [Required]
        [MaxLength( 100 )]
        [DataMember( IsRequired = true )]
        public string Name { get; set; }

        [DataMember]
        public int? RegistrationTemplateFormId { get; set; }

        [DataMember]
        public RegistrationAttributeAppliesTo AppliesTo { get; set; }

        [DataMember]
        public int? AttributeId { get; set; }

        [DataMember]
        public bool CommonValue { get; set; }

        [DataMember]
        public bool UseCurrentPersonAttributeValue { get; set; }

        [DataMember]
        public string PreText { get; set; }

        [DataMember]
        public string PostText { get; set; }

        [DataMember]
        public bool ShowOnGrid { get; set; }

        [DataMember]
        public bool RequiredOnInitialEntry { get; set; }

        #endregion

        #region Virtual Properties

        public virtual RegistrationTemplateForm RegistrationTemplateForm { get; set; }

        public virtual Attribute Attribute { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }

        #endregion

    }

    #region Entity Configuration

    /// <summary>
    /// Configuration class.
    /// </summary>
    public partial class RegistrationTemplateFormAttributeConfiguration : EntityTypeConfiguration<RegistrationTemplateFormAttribute>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationTemplateFormAttributeConfiguration"/> class.
        /// </summary>
        public RegistrationTemplateFormAttributeConfiguration()
        {
            this.HasRequired( a => a.RegistrationTemplateForm ).WithMany( t => t.FormAttributes ).HasForeignKey( i => i.RegistrationTemplateFormId ).WillCascadeOnDelete( true );
            this.HasRequired( a => a.Attribute ).WithMany().HasForeignKey( a => a.AttributeId ).WillCascadeOnDelete( false );
        }
    }

    #endregion

    #region Enumerations

    /// <summary>
    /// Represents how a <see cref="Rock.Model.Page"/> should be displayed in the page navigation controls.
    /// </summary>
    public enum RegistrationAttributeAppliesTo
    {
        /// <summary>
        /// Do NOT display field to user
        /// </summary>
        Person = 0,

        /// <summary>
        /// Display the field to user
        /// </summary>
        GroupMember = 1,

        /// <summary>
        /// Require user to enter the field value
        /// </summary>
        Registration = 2
    }

    #endregion
}
