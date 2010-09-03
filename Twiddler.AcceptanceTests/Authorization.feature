Feature: Authorization
	In order to interact with Twitter
	As a Twitter user
	I want to authorize Twidder with Twitter
	
Scenario: Not authorized
	Given I have not previously authorized
	Then the authorization status should show I am unauthorized
	
Scenario: Begin authorization
	Given I have not previously authorized
	When I click the Authorize button
	Then the authorization window should be displayed
