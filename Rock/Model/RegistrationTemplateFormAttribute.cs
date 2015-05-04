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

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        [MaxLength( 100 )]
        [DataMember( IsRequired = true )]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the registration template form identifier.
        /// </summary>
        /// <value>
        /// The registration template form identifier.
        /// </value>
        [DataMember]
        public int? RegistrationTemplateFormId { get; set; }

        /// <summary>
        /// Gets or sets the entity ( Person/GroupMember/Registrant ) that attribute applies to.
        /// </summary>
        /// <value>
        /// The applies to.
        /// </value>
        [DataMember]
        public RegistrationAttributeAppliesTo AppliesTo { get; set; }

        /// <summary>
        /// Gets or sets the attribute identifier.
        /// </summary>
        /// <value>
        /// The attribute identifier.
        /// </value>
        [DataMember]
        public int? AttributeId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this is a 'common value'. If so, the value entered will be auto set for each person on the registration.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [common value]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool CommonValue { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the current value of the person attribute if it already exists.
        /// </summary>
        /// <value>
        /// <c>true</c> if [use current person attribute value]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool UseCurrentPersonAttributeValue { get; set; }

        /// <summary>
        /// Gets or sets the pre text.
        /// </summary>
        /// <value>
        /// The pre text.
        /// </value>
        [DataMember]
        public string PreText { get; set; }

        /// <summary>
        /// Gets or sets the post text.
        /// </summary>
        /// <value>
        /// The post text.
        /// </value>
        [DataMember]
        public string PostText { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show on grid.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show on grid]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool ShowOnGrid { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to require on initial entry.
        /// </summary>
        /// <value>
        /// <c>true</c> if [required on initial entry]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool RequiredOnInitialEntry { get; set; }

        #endregion

        #region Virtual Properties

        /// <summary>
        /// Gets or sets the registration template form.
        /// </summary>
        /// <value>
        /// The registration template form.
        /// </value>
        public virtual RegistrationTemplateForm RegistrationTemplateForm { get; set; }

        /// <summary>
        /// Gets or sets the attribute.
        /// </summary>
        /// <value>
        /// The attribute.
        /// </value>
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
    /// The entity that attribute applies to
    /// </summary>
    public enum RegistrationAttributeAppliesTo
    {
        /// <summary>
        /// Person attribute
        /// </summary>
        Person = 0,

        /// <summary>
        /// Group Member attribute
        /// </summary>
        GroupMember = 1,

        /// <summary>
        /// Registration attribute
        /// </summary>
        Registration = 2
    }

    #endregion
}
