﻿using BlowTrial.Domain.Tables;
using BlowTrial.Properties;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace BlowTrial.Domain.Providers
{
    class MembershipContextInitialiser : MigrateDatabaseToLatestVersion<MembershipContext, BlowTrial.Migrations.Membership.MembershipConfiguration>
    {
    }
    public class DataContextInitialiser : MigrateDatabaseToLatestVersion<TrialDataContext, BlowTrial.Migrations.TrialData.TrialDataConfiguration>
    {
        public static readonly Vaccine RussianBcg =
            new Vaccine
            {
                Id = 1,
                Name = Strings.Vaccine_RussianBcg,
                IsBcg = true
            };
        public static readonly Vaccine Opv =
            new Vaccine
            {
                Id = 2,
                Name = Strings.Vaccine_Opv,
                IsBcg = false
            };
        public static readonly Vaccine HepB =
            new Vaccine
            {
                Id = 3,
                Name = Strings.Vaccine_HepB,
                IsBcg = false
            };
        public static readonly Vaccine DanishBcg =
            new Vaccine{
                Id = 5,
                Name=Strings.Vaccine_DanishBcg,
                IsBcg=true
            };
        public static readonly Vaccine BcgMoreau =
            new Vaccine
            {
                Id = 6,
                Name = Strings.Vaccine_BcgBrazil,
                IsBcg =true
            };
        public static readonly Vaccine BcgGreenSignal =
            new Vaccine
            {
                Id = 7,
                Name = Strings.Vaccine_GreenSignalBcg,
                IsBcg = true
            };
        public static readonly Vaccine BcgJapan =
            new Vaccine
            {
                Id = 8,
                Name = Strings.Vaccine_BcgJapan,
                IsBcg = true
            };

        public const int MaxReservedVaccineId = 20;

        public static int[] SeedVaccineIds(AllocationGroups group)
        {
            var returnList = new List<int>(5)
            {
                Opv.Id,
                HepB.Id
            };
            switch (group)
            {
                case AllocationGroups.NotApplicable:
                    throw new ArgumentException("NotApplicatble should never be used as an allocationGroup");
                case AllocationGroups.India2Arm:
                    returnList.Add(RussianBcg.Id);
                    break;
                case AllocationGroups.India3ArmBalanced:
                case AllocationGroups.India3ArmUnbalanced:
                    returnList.Add(RussianBcg.Id);
                    returnList.Add(DanishBcg.Id);
                    break;
                case AllocationGroups.Brazil2Arm:
                    returnList.Add(BcgMoreau.Id);
                    break;
                case AllocationGroups.Danish2Arm:
                    returnList.Add(DanishBcg.Id);
                    break;
                case AllocationGroups.GreenSignal2Arm:
                    returnList.Add(BcgGreenSignal.Id);
                    break;
                case AllocationGroups.Japan2Arm:
                    returnList.Add(BcgJapan.Id);
                    break;
                case AllocationGroups.Uganda3Arm:
                    returnList.Add(BcgJapan.Id);
                    returnList.Add(RussianBcg.Id);
                    break;
            }
            var returnVar = new int[returnList.Count];
            returnList.CopyTo(returnVar);
            return returnVar;
        }

    }
}
