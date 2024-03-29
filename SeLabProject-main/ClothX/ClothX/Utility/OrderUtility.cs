using ClothX.Constants;
using ClothX.DbModels;
using ClothX.Services;
using ClothX.ViewModels;

namespace ClothX.Utility
{
    // Utility class for handling client orders
    public class OrderUtility
    {
        // Singleton instance of the class
        private static OrderUtility _instance;

        // Property to access the singleton instance
        public static OrderUtility Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new OrderUtility();
                return _instance;
            }
        }

        // Private constructor to enforce singleton pattern
        private OrderUtility() { }

        // Get a list of client orders for a specific user
        public List<ClientOrder> getClientsOrders(string username = "")
        {
            ClothXDbContext db = new ClothXDbContext();
            var orders = db.ClientOrders.ToList();
            int userId = 0;
            if (username != "")
            {
                userId = UserUtility.Instance.getUserProfileId(username);
                orders = orders.Where(x => x.ClientId == userId).ToList();
            }
            return orders;
        }

        public List<ClientOrder> GetOrders(bool Month)
        {
            ClothXDbContext db = new ClothXDbContext();
            var orders = db.ClientOrders.Where(x => x.IsActive == true && x.AddedOn.Year == DateTime.Now.Year).ToList();
            if (Month == true)
            {
                orders = orders.Where(x => x.AddedOn.Month == DateTime.Now.Month).ToList();
            }

            return orders;
        }

        // Map a client order view model to a database model
        public ClientOrder getDbModel(ClientOrderViewModel model)
        {
            ClientOrder dbModel = new ClientOrder();
            dbModel.Title = model.Title;
            dbModel.Description = model.Description;
            dbModel.Measurements = model.Measurements;
            dbModel.OrderType = model.OrderType;
            dbModel.Price = 0;
            dbModel.Deadline = model.Deadline;
            return dbModel;
        }

        // Get a client order view model by order ID
        public ClientOrderViewModel getViewModel(int orderId)
        {
            ClothXDbContext db = new ClothXDbContext();
            var dbModel = db.ClientOrders.Where(x => x.Id == orderId).FirstOrDefault();

            ClientOrderViewModel model = new ClientOrderViewModel();
            model.Title = dbModel.Title;
            model.Description = dbModel.Description;
            model.Measurements = dbModel.Measurements;
            model.OrderType = dbModel.OrderType;
            model.OrderFeatures = dbModel.OrderFeatures;
            model.Deadline = dbModel.Deadline;
            model.ClientOrderIdeaImages = dbModel.ClientOrderIdeaImages;
            model.Price = dbModel.Price == null ? 0 : (int)dbModel.Price;
            model.Id = dbModel.Id;
            model.IsPaid = dbModel.IsPaid;
            model.IsConfirmed = dbModel.IsConfirmed;
            model.IsDelivered = dbModel.IsDelivered;
            model.ClientId = dbModel.ClientId;
            model.AddedBy = dbModel.AddedBy;
            model.AddedOn = dbModel.AddedOn;
            model.UpdatedOn = dbModel.UpdatedOn;
            model.IsActive = dbModel.IsActive;

            return model;
        }

        // Add a new client order to the database
        public async Task AddClientOrder(ClientOrderViewModel model, string username, IWebHostEnvironment webHost, IFormCollection form)
        {
            // Get database context
            ClothXDbContext db = new ClothXDbContext();

            // Create a new database model from the view model
            ClientOrder dbModel = getDbModel(model);

            // Get the user ID
            int userId = UserUtility.Instance.getUserProfileId(username);

            // Set additional properties
            dbModel.ClientId = userId;
            dbModel.AddedOn = DateTime.Now;
            dbModel.AddedBy = username;
            dbModel.UpdatedOn = DateTime.Now;
            dbModel.IsActive = true;
            dbModel.IsConfirmed = false;
            dbModel.IsDelivered = false;

            // Add the order to the database
            db.ClientOrders.Add(dbModel);
            db.SaveChanges();

            // Define file upload paths
            List<FilePathEnum> filepath = new()
            {
                FilePathEnum.Images,
                FilePathEnum.ProjectImages
            };

            // Upload idea images associated with the order
            if (model.ideaImages != null)
            {
                foreach (var s in model.ideaImages)
                {
                    ClientOrderIdeaImage img = new ClientOrderIdeaImage();
                    img.OrderId = dbModel.Id;
                    img.ImagePath = await UploadFileService.Instance.UploadFile(s, filepath, webHost);

                    await db.ClientOrderIdeaImages.AddAsync(img);
                    await db.SaveChangesAsync();
                }
            }

            // Get available features
            var features = FeaturesUtility.Instance.getFeatureGroups();

            int featuresPrice = 0;

            // Process selected features from the form
            foreach (var fg in features)
            {
                if (!string.IsNullOrEmpty(form["Feature-" + fg.Id]))
                {
                    int v = int.Parse(form["Feature-" + fg.Id]);

                    OrderFeature of = new OrderFeature();
                    of.OrderId = dbModel.Id;
                    of.FeatureId = v;
                    of.IsActive = true;
                    db.OrderFeatures.Add(of);
                    db.SaveChanges();

                    featuresPrice += db.Features.Where(x => x.Id == v).FirstOrDefault().Price;
                }
            }

            // Calculate and update the total price of the order
            dbModel.Price += db.ProductCategories.Where(x => x.Id == model.OrderType).FirstOrDefault().Price;
            dbModel.Price += featuresPrice;

            db.ClientOrders.Update(dbModel);
            db.SaveChanges();
        }

        // Edit an existing client order in the database
        public async Task EditClientOrder(ClientOrderViewModel model, string username, IWebHostEnvironment webHost, IFormCollection form)
        {

            ClothXDbContext db = new ClothXDbContext();
            ClientOrder clientOrder = db.ClientOrders.Where(x => x.Id == model.Id).FirstOrDefault();

            // Update ClientOrder properties with values from the provided model
            clientOrder.Title = model.Title;
            clientOrder.Description = model.Description;
            clientOrder.Measurements = model.Measurements;
            clientOrder.OrderType = model.OrderType;
            clientOrder.Deadline = model.Deadline;
            clientOrder.Price = 0;
            clientOrder.UpdatedOn = DateTime.Now;
            db.ClientOrders.Update(clientOrder);
            db.SaveChanges();

            // List of file paths for uploading images
            List<FilePathEnum> filepath = new()
{
    FilePathEnum.Images,
    FilePathEnum.ProjectImages
};


            if (model.ideaImages != null)
            {
                // Process each ideaImage and add it to the ClientOrderIdeaImages table
                foreach (var s in model.ideaImages)
                {
                    ClientOrderIdeaImage img = new ClientOrderIdeaImage();
                    img.OrderId = clientOrder.Id;
                    img.ImagePath = await UploadFileService.Instance.UploadFile(s, filepath, webHost);

                    await db.ClientOrderIdeaImages.AddAsync(img);
                    await db.SaveChangesAsync();
                }
            }

            // Retrieve the list of feature groups
            var features = FeaturesUtility.Instance.getFeatureGroups();


            int featuresPrice = 0;

            // Deactivate all existing OrderFeatures associated with the clientOrder
            foreach (var of in clientOrder.OrderFeatures)
            {
                of.IsActive = false;
                db.OrderFeatures.Update(of);
                db.SaveChanges();
            }

            // Process each feature group and update the associated OrderFeatures
            foreach (var fg in features)
            {
                if (!string.IsNullOrEmpty(form["Feature-" + fg.Id]))
                {
                    int v = int.Parse(form["Feature-" + fg.Id]);


                    if (clientOrder.OrderFeatures.Any(x => x.FeatureId == v))
                    {
                        // Activate the existing feature
                        var existingFeature = clientOrder.OrderFeatures.Where(x => x.FeatureId == v).FirstOrDefault();
                        existingFeature.IsActive = true;
                        db.OrderFeatures.Update(existingFeature);
                        db.SaveChanges();
                    }
                    else
                    {
                        // Create a new OrderFeature and add it to the OrderFeatures table
                        OrderFeature of = new OrderFeature();
                        of.OrderId = clientOrder.Id;
                        of.FeatureId = v;
                        of.IsActive = true;
                        db.OrderFeatures.Add(of);
                        db.SaveChanges();
                    }


                    featuresPrice += db.Features.Where(x => x.Id == v).FirstOrDefault().Price;
                }
            }

            clientOrder.Price += db.ProductCategories.Where(x => x.Id == model.OrderType).FirstOrDefault().Price;
            clientOrder.Price += featuresPrice;
            db.ClientOrders.Update(clientOrder);
            db.SaveChanges();

        }

        // Change the status of a client order (active or inactive)
        public void CHangeOrderStatus(int OrderId)
        {
            ClothXDbContext db = new ClothXDbContext();
            var dbModel = db.ClientOrders.Where(x => x.Id == OrderId).FirstOrDefault();
            dbModel.IsActive = !dbModel.IsActive;
            dbModel.UpdatedOn = DateTime.Now;
            db.ClientOrders.Update(dbModel);
            db.SaveChanges();
        }
    }
}
