# syncthing

syncthing performs is a media file management tool. It performs the following:

- media indexing and search
- media sync monitoring with external process triggers

## User Needs

### UN-001

Display one or more user selected directories to be indexed and displayed

### UN-002

Directory displays must be searchable

### UN-003

Watch for changes in one or more user selected directories and trigger external processes for:

- performing media organization
- triggering media center library refresh

### UN-004

When actions processes are running (i.e. indexing, organizing, library refreshing), the user shall be notified of:

- the current state of the operation (i.e. idle, busy, success, fail, etc.)
- useful process related information

The notification shall be:

- displayed for each process independently
- displayed in a unified notification

### UN-005

Manually trigger external process

### UN-006

All actions processes in the background as to not cause considerable delay

### UN-007

Only one action process of the same type is allowed at a time

## Requirements

### REQ-001

#### UN-001

The user shall be able to select directories to be indexed

### REQ-002

#### UN-001

Directories shall be displayed in a list showing file name and date created

### REQ-003

#### UN-001

The directory list shall be sortable by any of the displayed file attributes

### REQ-004

#### UN-001

When directories are being indexed, the state of the indexing shall be displayed

### REQ-005

#### UN-001

User interaction with directory lists while an index is in progress shall be prevented

### REQ-006

#### UN-001

Indexed directories shall automatically monitor for changes

### REQ-007

#### UN-001

Users shall be able to trigger a directory index manually

### REQ-008

#### UN-001

If permitted, users shall be able to open the parent directory of any item within the directory index

### REQ-009

#### UN-002

The user is able to filter directory index lists by the following criteria:

- text contained within filename

### REQ-010

#### UN-002

Search results must be displayed in real-time as the user types

### REQ-011

#### UN-002

Search text must be cleared easily by a single key/button (i.e. escape key)

### REQ-012

#### UN-003

User shall be able to select directories to be monitored for triggering

### REQ-013

#### UN-003

When monitored directory changes are detected, the application shall trigger media organization, followed by library update triggering

### REQ-014

#### UN-004

For each running external process, the application must display the current state of the process, where state is:

- idle
- active
- successful
- failed

### REQ-015

#### UN-004

During media organization external process, the application must display the following information about the process

- the file being organized

### REQ-016

#### UN-004

The application shall display a unified process indicator which indicates the following process actions:

- indexing
- organizing
- library refreshing

### REQ-017

#### UN-005

The user shall be able to manually trigger external processes for:

- organizing
- library refreshing

### REQ-018

#### UN-006

The application must perform action processes in the background as not to block the user during use

### REQ-019

#### UN-007

The application must control all action processes such that:

- directory indexing can not be triggered while the directory is being indexed
- organization can not be triggered while the organization is occurring
- library refresh can not be triggered while the library is being refreshed

### REQ-020

#### UN-003

The user shall be able to disable automatic triggering of any of the action processes

## Specifications

### SPEC-001

#### REQ-001

spec...

