Feature: Reply

Scenario: Create contact
	Given a user goes to Sales&Marketing->Contacts
	And a user creates new contact
	When a user opens latest contact
	Then data from form matches new contact data

Scenario: Run report
	Given a user goes to Reports&Settings->Reports
	And a user finds Project Profitability report
	When a user runs Project Profitability report
	Then that results are returned

Scenario: Remove events from activity log
	Given a user goes to Reports&Settings->ActivityLog
	And a user selects first 3 items in the table
	When a user deletes selected items in the table
	Then those items are deleted