const cors = require('cors')
const bodyParser = require('body-parser');
const MongoClient = require('mongodb').MongoClient;
const app = require('express')();

const uri = "mongodb+srv://tester:test555@thiencluster.dopd8nw.mongodb.net/?retryWrites=true&w=majority";
const client = new MongoClient(uri, { useNewUrlParser: true });

//Enable CORS
app.use(cors());

let pollsCollection;

// Connect to MongoDB and start server
client.connect(err => {
    if (err) {
        console.log("Error connecting to MongoDB:", err);
        return;
    }

    console.log("Connected to MongoDB");

    const db = client.db("pollsDB");

    pollsCollection = db.collection("polls");
})


//Enable CORS
app.use((req, res, next) => {
    res.header("Access-Control-Allow-Origin", "*");
    res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
    next();
});

app.use(bodyParser.json());

app.get('/polls', (req, res) => {
    pollsCollection.find().toArray((err, polls) => {
        if (err) {
            console.log(err);
            return res.status(500).send(err);
        }
        res.json({ polls });
    });
});



app.get('/polls/:id', (req, res) => {
    const pollId = parseInt(req.params.id);
    pollsCollection.findOne({ id: pollId }, (err, poll) => {
        if (err) {
            console.log(err);
            return res.status(500).send(err);
        }
        res.json(poll);
    });
});

app.post('/polls/:id/vote/:option', (req, res) => {
    const pollId = parseInt(req.params.id);
    const optionId = parseInt(req.params.option);
    pollsCollection.findOne({ id: pollId }, (err, poll) => {
        if (err) {
            console.log(err);
            return res.status(500).send(err);
        }
        const option = poll.options.find(opt => opt.id === optionId);
        if (!option) {
            return res.status(404).send('Option not found');
        }
        option.votes++;
        pollsCollection.replaceOne({ id: pollId }, poll, (err, result) => {
            if (err) {
                console.log(err);
                return res.status(500).send(err);
            }
            res.json(poll);
        });
    });
});

app.post('/polls/add', (req, res) => {
    const poll = {
        id: 0,
        title: req.body.title,
        options: req.body.options.map((opt, idx) => ({ id: idx + 1, title: opt, votes: 0 }))
    };

    pollsCollection.find().sort({ id: -1 }).limit(1).toArray((err, polls) => {
        if (err) {
            console.log(err);
            return res.status(500).send(err);
        }
        if (polls.length > 0) {
            poll.id = polls[0].id + 1;
        } else {
            poll.id = 1;
        }

        pollsCollection.insertOne(poll, (err, result) => {
            if (err) {
                console.log(err);
                return res.status(500).send(err);
            }
            res.json(poll);
        });
    });
});

const port = process.env.PORT ? process.env.PORT : 8081;
const server = app.listen(port, () => {
    console.log("Server listening  port %s", port);
});