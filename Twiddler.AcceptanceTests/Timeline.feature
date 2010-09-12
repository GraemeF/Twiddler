Feature: Timeline
	In order to not miss important Tweets
	As a Twitter user without enough time to read everything
	I want to see the most important Tweets in my timeline

Scenario: Uninteresting Tweets are ordered by time
	Given I have an empty Timeline
	When 3 uninteresting Tweets are retrieved
	Then all Tweets should be in descending order of time
