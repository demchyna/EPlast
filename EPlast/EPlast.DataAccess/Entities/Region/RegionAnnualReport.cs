﻿using System;

namespace EPlast.DataAccess.Entities
{
    public class RegionAnnualReport
    {
        public int ID { get; set; }
        public int NumberOfSeatsPtashat { get; set; }
        public int NumberOfPtashata { get; set; }
        public int NumberOfNovatstva { get; set; }
        public int NumberOfUnatstvaNoname { get; set; }
        public int NumberOfUnatstvaSupporters { get; set; }
        public int NumberOfUnatstvaMembers { get; set; }
        public int NumberOfUnatstvaProspectors { get; set; }
        public int NumberOfUnatstvaSkobVirlyts { get; set; }
        public int NumberOfSeniorPlastynSupporters { get; set; }
        public int NumberOfSeniorPlastynMembers { get; set; }
        public int NumberOfSeigneurSupporters { get; set; }
        public int NumberOfSeigneurMembers { get; set; }
        public int NumberOfIndependentRiy { get; set; }
        public DateTime Date { get; set; }
        public int NumberOfClubs { get; set; }
        public int NumberOfIndependentGroups { get; set; }
        public int NumberOfTeachers { get; set; }
        public int NumberOfAdministrators { get; set; }
        public int NumberOfTeacherAdministrators { get; set; }
        public int NumberOfBeneficiaries { get; set; }
        public int NumberOfPlastpryiatMembers { get; set; }
        public int NumberOfHonoraryMembers { get; set; }
        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public Region Region { get; set; }
    }
}