-- Add snapshot columns to store saved page JSON
ALTER TABLE PerformanceAgreements ADD PageSnapshotJson NVARCHAR(MAX) NULL;
ALTER TABLE MidTermReviews ADD PageSnapshotJson NVARCHAR(MAX) NULL;
ALTER TABLE AnnualAssessments ADD PageSnapshotJson NVARCHAR(MAX) NULL;

-- Add signature date columns to KRAs (if signatures are stored per-KRA)
ALTER TABLE KRAs ADD EmployeeSignatureDate DATETIME2 NULL, SupervisorSignatureDate DATETIME2 NULL;

-- Verify columns were added
SELECT TOP 10 * FROM PerformanceAgreements;
SELECT TOP 10 * FROM MidTermReviews;
SELECT TOP 10 * FROM AnnualAssessments;
SELECT TOP 10 * FROM KRAs;
