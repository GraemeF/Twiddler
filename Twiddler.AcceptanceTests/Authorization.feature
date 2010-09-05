Feature: Authorization
	In order to interact with Twitter
	As a Twitter user
	I want to authorize Twidder with Twitter
	
Scenario: Not authorized
	Given I have not previously authorized
	Then I should be unauthorized
	
Scenario: Twitter is unavailable for authorization
	Given I have not previously authorized
	And Twitter is unavailable
	When I authorize with Twitter
	Then Twitter should be unavailable

Scenario: Successfully authorize
	Given I have not previously authorized
	When I authorize with Twitter
	Then I should be authorized
