## Project Assumptions

1. TenantId is provided via JWT claim.
2. One learner belongs to one tenant.
3. One course belongs to one tenant.
4. Registration is tenant scoped.
5. Duplicate registrations are prevented through unique index.
6. Optimistic concurrency is implemented through RowVersion.
7. Audit logs are created only for successful registrations.
