using System;
using System.Collections.Generic;

namespace DataLayer.Models.EntityModels
{
    public partial class TaskView
    {
        public int id { get; set; }
        public string? firstName { get; set; }
        public string? lastName { get; set; }
        public string? fullName { get; set; }
        public string? uniqueId { get; set; }
        public bool? qualityInspectors { get; set; }
        public bool? assosicateInspectors { get; set; }
        public bool? planEstimateInspectors { get; set; }
        public bool? customerInspectors { get; set; }
        public string? aor { get; set; }
        public string? positionTitle { get; set; }
        public string? email { get; set; }
        public string? accessType { get; set; }
        public bool? activeStatus { get; set; }
        public bool? fullAdminRights { get; set; }
        public bool? editRights { get; set; }
        public bool? deleteRights { get; set; }
        public bool? viewRights { get; set; }
        public bool? importRights { get; set; }
        public bool? activateAutologout { get; set; }
        public string? loginId { get; set; }
        public string? password { get; set; }
        public bool? resetPassword { get; set; }
        public string? securityQuesOne { get; set; }
        public string? securityAnsOne { get; set; }
        public DateTime? dateAccessGranted { get; set; }
        public DateTime? dateAccessRemoved { get; set; }
        public string? comments { get; set; }
    }
}
