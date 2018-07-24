# ManageNN
Workflow assembly for managing Many-Many relationships in Dynamics CRM 2016

To use: Import DLL using Plugin Registration Utility in the SDK

I found it ost flexible to create an Action:
  Input: Record URL for the entity being called by a workflow
  Call the workflow assembly
  Output: passes Success value from the workflow

Then call the action with your workflow
