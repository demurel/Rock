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
using System;
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
    [Table( "RegistrationInstance" )]
    [DataContract]
    public partial class RegistrationInstance : Model<RegistrationInstance>
    {

        #region Entity Properties

        [Required]
        [MaxLength( 100 )]
        [DataMember( IsRequired = true )]
        public string Name { get; set; }

        [DataMember]
        public int? RegistrationTemplateId { get; set; }

        [DataMember]
        public DateTime? StartDateTime { get; set; }

        [DataMember]
        public DateTime? EndDateTime { get; set; }

        [DataMember]
        public string Details { get; set; }

        [DataMember]
        public int MaxAttendees { get; set; }

        [DataMember]
        [MaxLength( 100 )]
        public string AccountCode { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        [MaxLength( 200 )]
        public string ContactName { get; set; }

        [DataMember]
        [MaxLength( 200 )]
        public string ContactEmail { get; set; }

        [DataMember]
        public string AdditionalReminderDetails { get; set; }

        [DataMember]
        public string AdditionalConfirmationDetails { get; set; }

        [DataMember]
        public DateTime? ReminderSentDateTime { get; set; }

        [DataMember]
        public DateTime? ConfirmationSentDateTime { get; set; }

        #endregion

        #region Virtual Properties

        public virtual RegistrationTemplate RegistrationTemplate { get; set; }

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
    public partial class RegistrationInstanceConfiguration : EntityTypeConfiguration<RegistrationInstance>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationTemplateConfiguration"/> class.
        /// </summary>
        public RegistrationInstanceConfiguration()
        {
            this.HasRequired( i => i.RegistrationTemplate ).WithMany( t => t.Instances).HasForeignKey( i => i.RegistrationTemplateId ).WillCascadeOnDelete( true );
        }
    }

    #endregion

}
