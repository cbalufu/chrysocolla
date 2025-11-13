# Forums

## Overview
The Forums capability provides community discussion platform with topics, posts, threaded replies, moderation, and engagement features like views, likes, pinning, and locking.

## Requirements

### Requirement: Forum Topics
The system SHALL organize discussions into topics/categories with name and description.

#### Scenario: Create forum topic
- **WHEN** creating forum topic "Community Events"
- **THEN** the system creates topic with status active
- **AND** initializes post count to zero
- **AND** makes available for posts

### Requirement: Forum Posts
The system SHALL allow citizens to create posts within topics with title and content.

#### Scenario: Create forum post
- **WHEN** citizen creates post in topic
- **THEN** the system creates post with status "Active"
- **AND** links to topic and citizen
- **AND** initializes view, reply, and like counts to zero
- **AND** increments topic post count

#### Scenario: View post
- **WHEN** citizen views a forum post
- **THEN** the system increments view count
- **AND** displays post content and metadata

### Requirement: Post Replies
The system SHALL support threaded replies to forum posts.

#### Scenario: Reply to post
- **WHEN** citizen replies to forum post
- **THEN** the system creates `ForumReply` record
- **AND** links to post and citizen
- **AND** increments post reply count
- **AND** displays in chronological order

#### Scenario: Reply to reply (nested threads)
- **WHEN** citizen replies to another reply
- **THEN** the system creates reply linked to parent reply
- **AND** maintains thread hierarchy

### Requirement: Post Moderation
The system SHALL support moderation with post statuses: Active, Pending, Rejected, Flagged.

#### Scenario: Flag inappropriate post
- **WHEN** user flags post as inappropriate
- **THEN** the system updates status to "Flagged"
- **AND** notifies moderators
- **AND** queues for review

#### Scenario: Moderate flagged post
- **WHEN** moderator reviews flagged post
- **THEN** moderator can approve (status: "Active") or reject (status: "Rejected")
- **AND** rejected posts are hidden from public view

#### Scenario: Pending post approval
- **WHEN** creating post in moderated topic
- **THEN** the system sets status to "Pending"
- **AND** requires moderator approval before display

### Requirement: Post Management
The system SHALL allow moderators to pin posts for visibility and lock posts to prevent replies.

#### Scenario: Pin important post
- **WHEN** moderator pins post
- **THEN** the system sets `IsPinned` to true
- **AND** displays post at top of topic
- **AND** highlights visually

#### Scenario: Lock post
- **WHEN** moderator locks post
- **THEN** the system sets `IsLocked` to true
- **AND** prevents new replies
- **AND** displays lock indicator

### Requirement: Engagement Features
The system SHALL track view counts, reply counts, and likes for posts and replies.

#### Scenario: Like post
- **WHEN** citizen likes a post
- **THEN** the system increments like count
- **AND** records citizen's like
- **AND** prevents duplicate likes from same citizen

#### Scenario: Unlike post
- **WHEN** citizen unlikes previously liked post
- **THEN** the system decrements like count
- **AND** removes like record

### Requirement: Search and Filtering
The system SHALL provide search within forums by keyword, topic, and citizen.

#### Scenario: Search posts by keyword
- **WHEN** searching for "recycling"
- **THEN** the system returns posts containing keyword in title or content
- **AND** orders by relevance or date

#### Scenario: View citizen's posts
- **WHEN** viewing all posts by specific citizen
- **THEN** the system returns posts authored by that citizen
- **AND** respects moderation status

### Requirement: Multi-tenant Isolation
The system SHALL ensure complete data isolation between tenants.

#### Scenario: Tenant isolation
- **WHEN** querying forum posts from Tenant A
- **THEN** the system returns only posts from Tenant A

## Data Model

### ForumTopic Entity
- **Id**: Guid
- **TenantId**: Guid?
- **Name**: string
- **Description**: string
- **PostCount**: int
- **IsActive**: bool
- **Posts**: ICollection<ForumPost>
- Audit fields

### ForumPost Entity
- **Id**: Guid
- **TenantId**: Guid?
- **ForumTopicId**: Guid (FK)
- **CitizenId**: Guid (FK)
- **Title**: string
- **Content**: string
- **Status**: ForumPostStatus enum
- **ViewCount**: int
- **ReplyCount**: int
- **LikeCount**: int
- **IsPinned**: bool
- **IsLocked**: bool
- **Replies**: ICollection<ForumReply>
- Audit fields

### ForumReply Entity
- **Id**: Guid
- **ForumPostId**: Guid (FK)
- **CitizenId**: Guid (FK)
- **Content**: string
- **LikeCount**: int
- **Status**: ForumPostStatus enum
- Audit fields

### ForumPostStatus Enum
- Active, Pending, Rejected, Flagged

## Implementation Notes
- Located in `src/CitizensPortal.Application/Services/ForumAppService.cs`
- Entities in `src/CitizensPortal.Domain/Entities/Forum.cs`
- Enums in `src/CitizensPortal.Domain.Shared/Enums/CommunityEnums.cs`
- HTTP API: No controller yet (needs to be created)
- Full-text search capabilities needed for post search
- Moderation queue for pending/flagged content
