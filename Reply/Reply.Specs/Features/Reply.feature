Feature: Reply

Scenario: Create contact
	Given I go to Sales&Marketing->Contacts
	And I create new contact
	When I open latest contact
	Then I check if data is correct

Scenario: Run report
	Given I go to Reports&Settings->Reports
	And I find Project Profitability report
	When I run Project Profitability report
	Then I verify that results were returned

Scenario: Remove events from activity log
	Given I go to Reports&Settings->ActivityLog
	And I select first 3 items in the table
	When I delete first 3 items in the table
	Then I verify that items were deleted