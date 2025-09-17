from .bases import ExocolonistTestBase

class TestJobUnlockLogic(ExocolonistTestBase):
    options = {
        "friendsanity": False,
        "datesanity": False,
        "ending": 0,
        "perksanity": false,
    }

    def test_job_access(self) -> None:
        self.assertAccessDependency(
            ["Tutoring"],
            [["Tutoring"], ["Study Engineering"]],
            only_check_listed=True,
        )
        