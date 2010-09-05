Feature: Authorization
	In order to interact with Twitter
	As a Twitter user
	I want to authorize Twidder with Twitter
	
Scenario: Not authorized
	Given I have not previously authorized
	Then I should be unauthorized
	
Scenario: Successfully authorize
	Given I have not previously authorized
	When I authorize with Twitter
	Then I should be authorized
