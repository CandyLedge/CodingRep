-- 创建用户表
create table users (
                       id int primary key identity(1,1),
                       userName nvarchar(100) not null unique,
                       passwordHash nvarchar(256) not null,
                       salt nvarchar(50) not null default '',
                       email nvarchar(100) not null unique,
                       avatarUrl nvarchar(256) null default '',
                       createdAt datetime not null default getdate()
);

-- 创建仓库表
create table repositories (
                              id int primary key identity(1,1),
                              userId int not null,
                              name nvarchar(100) not null,
                              description nvarchar(500),
                              createdAt datetime not null default getdate(),
                              isPrivate bit not null default 0,
                              foreign key (userId) references users(id),
                              unique (userId, name)
);

-- 创建提交记录表
create table commits (
                         id int primary key identity(1,1),
                         repoId int not null,
                         userId int not null,
                         message nvarchar(256),
                         parentId int null,
    [timestamp] datetime not null default getdate(),
    foreign key (repoId) references repositories(id),
    foreign key (parentId) references commits(id),
    foreign key (userId) references users(id)
    );

-- 提交记录索引
create index idx_commits_repoId on commits(repoId);
create index idx_commits_parentId on commits(parentId);
create index idx_commits_timestamp on commits([timestamp]);

-- 创建分支表
create table branches (
                          id int primary key identity(1,1),
                          repoId int not null,
                          name nvarchar(100) not null,
                          commitId int,
                          createdAt datetime not null default getdate(),
                          foreign key (repoId) references repositories(id),
                          foreign key (commitId) references commits(id),
                          unique (repoId, name)
);

-- 分支表索引
create index idx_branches_repoId on branches(repoId);
create index idx_branches_commitId on branches(commitId);

-- 创建文件快照表
create table fileSnapshots (
                               id int primary key identity(1,1),
                               commitId int not null,
                               path nvarchar(200) not null,
                               content varbinary(max),
                               contentHash nvarchar(40) null,
                               fileMode int not null default 0,
                               createdAt datetime not null default getdate(),
                               foreign key (commitId) references commits(id),
                               unique (commitId, path)
);

-- 文件快照表索引
create index idx_fileSnapshots_commitId on fileSnapshots(commitId);
create index idx_fileSnapshots_path on fileSnapshots(path);