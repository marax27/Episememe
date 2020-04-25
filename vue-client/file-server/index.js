
function main() {
    const express = require('express');
    const app = express();
    const port = 18888;

    app.use(express.static('media'));
    app.listen(port, () => console.log(`Static file server listening at http://localhost:${port}`));
}

main();
