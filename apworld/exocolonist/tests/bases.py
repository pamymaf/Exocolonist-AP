from test.bases import WorldTestBase

from ..world import ExocolonistWorld

class ExocolonistTestBase(WorldTestBase):
    game = "Exocolonist"
    world: ExocolonistWorld