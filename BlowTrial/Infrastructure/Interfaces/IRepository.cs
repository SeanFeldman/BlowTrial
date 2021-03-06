﻿using BlowTrial.Domain.Outcomes;
using BlowTrial.Domain.Providers;
using BlowTrial.Domain.Tables;
using BlowTrial.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace BlowTrial.Infrastructure.Interfaces
{
    [Flags]
    public enum UpdateParticipantViolationType
    {
        NoViolations = 0,
        BlockCriteriaChanged = 1,
        IneligibleWeight = 2,
        MultipleSiblingIdChanged = 4
    }
    public interface IRepository : IDisposable
    {
        event EventHandler<ParticipantEventArgs> ParticipantAdded;
        event EventHandler<ScreenedPatientEventArgs> ScreenedPatientAdded;
        event EventHandler<ParticipantEventArgs> ParticipantUpdated;
        event EventHandler<ProtocolViolationEventArgs> ProtocolViolationAddOrUpdate;
        event EventHandler<FailedRestoreEvent> FailedDbRestore;
        event EventHandler<LastUpdatedChangedEventAgs> AnyParticipantChange;
        event EventHandler<DatabaseUpdatingEventAgs> DatabaseUpdating;
        event ProgressChangedEventHandler UpdateProgress;
        event EventHandler StudySiteAddOrUpdate;
        //event EventHandler<ScreenedPatientEventArgs> ScreenedPatientUpdated;
        Participant AddParticipant(
            string name,
            string mothersName,
            string hospitalIdentifier,
            int admissionWeight,
            double gestAgeBirth,
            DateTime dateTimeBirth,
            string AdmissionDiagnosis,
            string phoneNumber,
            bool isMale,
            bool? inborn,
            DateTime registeredAt,
            int centreId,
            MaternalBCGScarStatus maternalBCGScar,
            int? multipleSiblingId,
            int? envelopeNumber = null);
        void Add(ScreenedPatient patient);
        void Add(Vaccine newVaccine);
        void UpdateParticipant(int id,
            CauseOfDeathOption causeOfDeath,
            string otherCauseOfDeathDetail,
            bool? bcgAdverse,
            string bcgAdverseDetail,
            bool? bcgPapuleAtDischarge,
            bool? bcgPapuleAt28days,
            int? lastContactWeight,
            DateTime? lastWeightDate,
            DateTime? dischargeDateTime,
            DateTime? deathOrLastContactDateTime,
            OutcomeAt28DaysOption outcomeAt28Days,
            MaternalBCGScarStatus maternalBCGScar,
            FollowUpBabyBCGReactionStatus followUpBabyBCGReaction,
            DateTime? followUpContactMade,
            bool permanentlyUncontactable,
            string notes,
            IEnumerable<VaccineAdministered> vaccinesAdministered=null,
            IEnumerable<UnsuccessfulFollowUp> unsuccesfulFollowUps=null);
        UpdateParticipantViolationType UpdateParticipant(int id,
            string name,
            bool isMale,
            string phoneNumber,
            string AdmissionDiagnosis,
            int admissionWeight,
            DateTime dateTimeBirth,
            double gestAgeBirth,
            string hospitalIdentifier,
            bool? isInborn,
            int? multipleSibblingId,
            MaternalBCGScarStatus maternalBCGScar,
            DateTime registeredAt,
            bool isEnvelopeRandomising);
        //void AddOrUpdateVaccinesFor(int participantId, IEnumerable<VaccineAdministered> vaccinesAdministered);
        void Update(IEnumerable<Participant> patients);
        void Update(Participant patient);
        void Update(ScreenedPatient patient);
        void AddOrUpdate(ProtocolViolation violation);
        void AddOrUpdate(IEnumerable<StudyCentre> centres);
        DbQuery<Participant> Participants { get; }
        DbQuery<ScreenedPatient> ScreenedPatients { get; }
        DbQuery<VaccineAdministered> VaccinesAdministered { get; }
        DbQuery<UnsuccessfulFollowUp> UnsuccessfulFollowUps { get; }
        IEnumerable<Vaccine> Vaccines { get; }
        DbQuery<ProtocolViolation> ProtocolViolations { get; }
        Participant FindParticipantAndCollections(int participantId);
        Participant FindParticipant(int participantId);
        ProtocolViolation FindViolation(int violationId);
        IEnumerable<string> CloudDirectories { get; set; }
        ICollection<StudyCentreModel> LocalStudyCentres { get; }
        ParticipantsSummary GetParticipantSummary();
        ScreenedPatientsSummary GetScreenedPatientSummary();
        ICollection<StudyCentreModel> GetCentresRequiringData();
        StudyCentreModel FindStudyCentre(int studyCentreId);
        DateTime? LastCreateModifyParticipant();
        string BackupLimitedDbTo(string directory, params StudyCentreModel[] studyCentres);
        IEnumerable<KeyValuePair<string, IEnumerable<StudyCentreModel>>> GetFilenamesAndCentres();
        Database Database { get; }
        void Backup();
        void Restore();
    }
}
